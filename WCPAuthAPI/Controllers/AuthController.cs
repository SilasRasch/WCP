using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WCPAuthAPI.Models.DTOs;
using WCPShared.Interfaces;
using WCPShared.Services.StaticHelpers;
using WCPShared.Services;
using System.Net;
using WCPShared.Interfaces.Auth;
using WCPShared.Services.EntityFramework;
using SendGrid.Helpers.Errors.Model;
using WCPShared.Models.Entities.AuthModels;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Models.Enums;

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
                if (user is not null)
                    return user.Role != UserRole.Bruger ? Created($"auth/{user.Id}", user.Id) : Created($"auth/{user.Id}", new { id = user.Id, orgId = user.OrganizationId });
                return BadRequest("Something went wrong...");

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST /auth/verify
        [HttpPost("Verify"), AllowAnonymous]
        public async Task<IActionResult> Verify(VerifyUserDto request)
        {
            try
            {
                var response = await _authService.Verify(request);
                return response is not null ? Ok() : BadRequest("Something went wrong...");
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(NotFoundException))
                    return NotFound();
                else return BadRequest(ex.Message);
            }
        }

        // POST /auth/verify
        [HttpPost("SelfRegister"), AllowAnonymous]
        public async Task<IActionResult> SelfRegister(SelfRegisterDto request)
        {
            try
            {
                var response = await _authService.SelfRegister(request);
                return response is not null ? NoContent() : BadRequest("Something went wrong...");
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(NotFoundException))
                    return NotFound();
                else return BadRequest(ex.Message);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Authenticate"), Authorize]
        public async Task<ActionResult<string>> Authenticate()
        {
            try
            {
                var response = await _authService.Authenticate();
                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(NotFoundException))
                    return NotFound();
                else return BadRequest(ex.Message);
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

            //var status = await _emailService.SendForgotPasswordEmail(user, token);
            return Ok();
            //return status == HttpStatusCode.Accepted ? Ok() : BadRequest("Email could not be sent...");
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

            return Ok(new { email = user.Email, role = user.Role, id = user.Id });
        }

        [HttpPost("AddAdmin"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAdmin(int id)
        {
            return await _authService.AddAdmin(id) ? NoContent() : BadRequest("Der blev ikke fundet nogen bruger med det id. Eller brugeren er allerede admin...");
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

    }
}
