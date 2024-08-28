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

namespace WCPAuthAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _tokenService;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly UserContextService _userContextService;
        private readonly CookieOptions cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(1),
            Path = "/",
            Secure = true,
            HttpOnly = Secrets.IsProd,
            SameSite = Secrets.IsProd ? SameSiteMode.Strict : SameSiteMode.None,
        };

        public AuthController(IJwtService tokenService, IAuthService authService, IUserService userService, IEmailService emailService, UserContextService userContextService)
        {
            _tokenService = tokenService;
            _userService = userService;
            _emailService = emailService;
            _userContextService = userContextService;
            _authService = authService;
        }

        [HttpPost("Register"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> Register(RegisterDto request)
        {
            if (!request.Validate())
                return BadRequest("Valideringsfejl, tjek venligst felterne igen...");

            User user = await _authService.Register(request);

            if (user is null) return BadRequest("Der eksisterer allerede en bruger med denne email...");

            return user.Role != "Bruger" ? Created($"auth/{user.Id}", user.Id) : Created($"auth/{user.Id}", new { id = user.Id, orgId = user.OrganizationId });
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string?>> Login(UserDto request)
        {
            AuthResponse? auth = await _authService.Login(request);
            if (auth == null) return BadRequest("Forkert brugernavn eller kodeord");

            if (!await _authService.CheckLoginAttempts(request))
                return BadRequest("Du er blevet midlertidigt udelukket grundet for mange mislykkede loginforsøg");

            Response.Cookies.Append(Secrets.RefreshTokenCookieName, auth.RefreshToken, cookieOptions);

            return Ok(auth.Token);
        }

        [HttpPost("Refresh")]
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
            User? user = await _userService.GetUserByEmail(email);

            if (user is null)
                return BadRequest("No user with the given email");

            var id = user.Id;
            var roles = _userContextService.GetRoles();
            var displayName = user.DisplayName;
            var phone = user.Phone;
            var orgId = 0;

            if (user.Organization is not null)
                orgId = user.Organization.Id;

            if (orgId == 0)
                return Ok(new { id, email, roles, displayName, phone });

            return Ok(new { id, orgId, email, roles, displayName, phone });
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
        [HttpPost("Verify")]
        public async Task<IActionResult> Verify(VerifyUserDTO request)
        {
            var user = await _userService.GetUserByVerificationToken(request.VerificationToken);

            if (user == null) return BadRequest();

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.PasswordHash = passwordHash;
            user.IsActive = true;

            await _userService.UpdateObject(user.Id, user);

            return Ok();
        }

        [HttpPut("Reset-password"), AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            User? user = await _userService.GetUserByVerificationToken(request.Token);

            if (user == null) return BadRequest();
            if (user.PasswordResetToken != request.Token || user.ResetTokenExpiry < DateTime.Now) return BadRequest();

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
            var user = await _userService.GetUserByEmail(email.Email);

            if (user is null) return BadRequest();

            var token = _tokenService.GenerateRandomString(64);
            user.PasswordResetToken = token;
            user.ResetTokenExpiry = DateTime.Now.AddMinutes(30);
            await _userService.UpdateObject(user.Id, user);

            var status = await _emailService.SendForgotPasswordEmail(user, token);

            return status == HttpStatusCode.Accepted ? Ok() : BadRequest();
        }

        // PUT api/<UsersController>/5 - Used when changing password while already authenticated
        [HttpPost("Change-password/{id}")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] PasswordOnly request)
        {
            User? user = await _userService.GetObject(id);

            if (user == null) return BadRequest();
            if (_userContextService.GetId() != id && !_userContextService.GetRoles().Contains("Admin"))
                return Unauthorized();

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.PasswordHash = passwordHash;
            await _userService.UpdateObject(user.Id, user);

            return Ok();
        }
    }
}
