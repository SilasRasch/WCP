using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WCPAuthAPI.Models.DTOs;
using WCPShared.Interfaces;
using WCPShared.Models.UserModels;
using WCPShared.Services.StaticHelpers;
using WCPShared.Services;
using System.Net;
using WCPShared.Models.AuthModels;
using WCPShared.Interfaces.Auth;
using WCPShared.Models.DTOs;
using WCPShared.Services.EntityFramework;

namespace WCPAuthAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _tokenService;
        private readonly IAuthService _authService;
        private readonly UserService _userService;
        private readonly IEmailService _emailService;
        private readonly CreatorService _creatorService;
        private readonly UserContextService _userContextService;
        private readonly CookieOptions cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(1),
            Path = "/",
            Secure = true,
            HttpOnly = Secrets.IsProd,
            SameSite = Secrets.IsProd ? SameSiteMode.Strict : SameSiteMode.None,
        };

        public AuthController(IJwtService tokenService, IAuthService authService, UserService userService, IEmailService emailService, CreatorService creatorService, UserContextService userContextService)
        {
            _tokenService = tokenService;
            _userService = userService;
            _emailService = emailService;
            _creatorService = creatorService;
            _userContextService = userContextService;
            _authService = authService;
        }

        [HttpPost("Register"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> Register(RegisterCreatorDto request, [FromQuery] bool selfRegister = false)
        {
            if (!request.User.Validate())
                return BadRequest("Valideringsfejl på bruger, tjek venligst felterne igen...");

            if (request.Creator is not null && (request.User.Role == "Creator" || request.User.Role == "Editor") && !request.Creator.Validate())
                return BadRequest("Valideringsfejl på creator, tjek venligst felterne igen...");

            try
            {
                User? user = await _authService.Register(request, selfRegister);
                return user.Role != "Bruger" ? Created($"auth/{user.Id}", user.Id) : Created($"auth/{user.Id}", new { id = user.Id, orgId = user.OrganizationId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login"), AllowAnonymous]
        public async Task<ActionResult<string?>> Login(UserDto request)
        {
            try
            {
                AuthResponse? auth = await _authService.Login(request);
                if (auth == null) return BadRequest("Forkert brugernavn eller kodeord");

                if (!await _authService.CheckLoginAttempts(request))
                    return BadRequest("Du er blevet midlertidigt udelukket grundet for mange mislykkede loginforsøg");

                Response.Cookies.Append(Secrets.RefreshTokenCookieName, auth.RefreshToken, cookieOptions);

                return Ok(auth.Token);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Refresh"), AllowAnonymous]
        public async Task<ActionResult<string?>> RefreshToken([FromQuery] int? id = null, [FromQuery] string? email = null)
        {
            var refreshToken = Request.Cookies[Secrets.RefreshTokenCookieName];

            if (String.IsNullOrEmpty(refreshToken)) return BadRequest("Credentials not found");

            string? auth = null;

            if (id is not null)
                auth = await _tokenService.RefreshToken(id.Value, refreshToken);

            if (email is not null)
                auth = await _tokenService.RefreshToken(email, refreshToken);

            if (auth is null) return BadRequest("Credentials invalid");
            return Ok(auth);
        }

        [HttpGet("Authenticate"), Authorize]
        public async Task<ActionResult<string>> Authenticate()
        {
            string email = _userContextService.GetEmail();
            User? user = await _userService.GetObjectBy(x => x.Email == email);

            if (user is null)
                return BadRequest("No user with the given email");

            var id = user.Id;
            var roles = _userContextService.GetRoles();
            var name = user.Name;
            var phone = user.Phone;
            var notificationSetting = user.NotificationSetting;

            if (user.Role == "Bruger" && user.Organization is not null)
            {
                int orgId = user.Organization.Id;
                return Ok(new { id, orgId, email, roles, name, phone, notificationSetting });
            }
            
            if (user.Role == "Creator")
            {
                var creator = await _creatorService.GetObjectViewBy(x => x.UserId == _userContextService.GetId());
                if (creator is not null)
                {
                    var creatorId = creator.Id;
                    return Ok(new { id, creatorId, email, roles, name, phone, notificationSetting });
                }
            }

            return Ok(new { id, email, roles, name, phone, notificationSetting });
        }

        [HttpPost("AddAdmin"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAdmin(int id)
        {
            return await _tokenService.AddAdmin(id) ? NoContent() : BadRequest("Der blev ikke fundet nogen bruger med det id. Eller brugeren er allerede admin...");
        }

        [HttpPost("Revoke"), Authorize]
        public async Task<IActionResult> Revoke()
        {
            try
            {
                int id = _userContextService.GetId();
                await _tokenService.RevokeSession(id);

                return NoContent();
            }
            catch
            {
                return BadRequest("Session invalid");
            }
        }

        // POST /auth/verify
        [HttpPost("Verify"), AllowAnonymous]
        public async Task<IActionResult> Verify(VerifyUserDTO request)
        {
            var user = await _userService.GetObjectBy(x => x.VerificationToken == request.VerificationToken);
            if (user == null) return BadRequest();

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.PasswordHash = passwordHash;
            user.IsActive = true;

            await _userService.UpdateObject(user.Id, user);

            return Ok();
        }

        // POST /auth/verify
        [HttpPost("SelfRegister"), AllowAnonymous]
        public async Task<IActionResult> SelfRegister(RegisterSelfDto request)
        {
            var user = await _userService.GetObjectBy(x => x.VerificationToken == request.VerificationToken);
            if (user == null) return BadRequest();

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.PasswordHash = passwordHash;
            user.Phone = request.User.Phone;
            user.Name = request.User.Name;
            user.IsActive = true;

            if (!user.Validate()) return BadRequest("Validation failed...");

            var updateResult = await _userService.UpdateObject(user.Id, user);
            if (updateResult is null) return BadRequest("Updating user failed...");

            if (user.Role == "Creator" && request.Creator is not null)
            {
                if (!request.Creator.Validate()) return BadRequest("Creator validation failed");

                var result = await _creatorService.AddObject(request.Creator);
                if (result is null) return BadRequest("Something went wrong...");
            }

            user.VerificationToken = null;
            updateResult = await _userService.UpdateObject(user.Id, user);

            return updateResult is not null ? Ok() : BadRequest("Verification token removal failed");
        }

        [HttpPut("Reset-password"), AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            User? user = await _userService.GetObjectBy(x => x.PasswordResetToken == request.Token);

            if (user == null) return BadRequest("Forkert reset token, start venligst forfra");
            if (user.PasswordResetToken != request.Token) return BadRequest("Reset token mismatch...");
            if (user.ResetTokenExpiry < DateTime.Now) return BadRequest("Reset token forældet, start venligst forfra");

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.PasswordHash = passwordHash;
            user.ResetTokenExpiry = null;
            user.PasswordResetToken = null;
            await _userService.UpdateObject(user.Id, user);

            return Ok();
        }

        [HttpPost("Forgot-password"), AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(EmailOnly email)
        {
            var user = await _userService.GetObjectBy(x => x.Email == email.Email);

            if (user is null) return BadRequest();

            var token = _tokenService.GenerateRandomString(64);
            user.PasswordResetToken = token;
            user.ResetTokenExpiry = DateTime.Now.AddMinutes(30);
            await _userService.UpdateObject(user.Id, user);

            var status = await _emailService.SendForgotPasswordEmail(user, token);

            return status == HttpStatusCode.Accepted ? Ok() : BadRequest("Email could not be sent...");
        }

        // PUT api/<UsersController>/5 - Used when changing password while already authenticated
        [HttpPost("Change-password/{id}"), Authorize]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] PasswordOnly request)
        {
            User? user = await _userService.GetObject(id);

            if (user == null) return BadRequest();
            if (_userContextService.GetId() != id && !_userContextService.GetRoles().Contains("Admin"))
                return Unauthorized("Adgang nægtet...");

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.PasswordHash = passwordHash;
            await _userService.UpdateObject(user.Id, user);

            return Ok();
        }

        [HttpPost("NotificationSettings"), Authorize]
        public async Task<IActionResult> UpdateNotificationSettings([FromQuery] int userId, [FromQuery] string setting)
        {
            User? user = await _userService.GetObject(userId);
            if (user == null) return BadRequest();

            if (_userContextService.GetId() != userId && !_userContextService.GetRoles().Contains("Admin"))
                return Unauthorized("Adgang nægtet...");
            
            setting = setting.ToLower();
            if (setting != "slack" && setting != "email" && setting != "off")
                return BadRequest("Setting not accepted");
            
            user.NotificationSetting = setting;
            user.NotificationsOn = setting == "off" ? false : true;  

            await _userService.UpdateObject(user.Id, user);
            return Ok();
        }

        [HttpGet("UserToVerify/{verificationToken}"), AllowAnonymous]
        public async Task<ActionResult<string>> GetUserToVerify(string verificationToken)
        {
            var user = await _userService.GetObjectBy(x => x.VerificationToken == verificationToken);
            if (user == null) return NotFound("No user found with the given verification token...");

            return Ok(new { email = user.Email, role = user.Role });
        }
    }
}
