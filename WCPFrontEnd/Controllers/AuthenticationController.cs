using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WCPFrontEnd.Services;
using WCPShared.Models.Entities.AuthModels;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Models.Enums;
using WCPShared.Services;
using WCPShared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using WCPShared.Interfaces.Auth;
using WCPShared.Services.StaticHelpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WCPFrontEnd.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IWcpDbContext Context;
        private StripeService StripeService;

        public AuthenticationController(IWcpDbContext context, StripeService stripeService)
        {
            Context = context;
            StripeService = stripeService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserDto request)
        {
            User? user = await Context.Users
                .Include(x => x.Organization)
                .SingleOrDefaultAsync(x => x.Email == request.Email);

            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Invalid email or password");
            }

            if (!user.IsActive)
            {
                return BadRequest("User deactivated");
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            if (user.Role == UserRole.Bruger && (user.Organization is null || !user.Organization.IsActive || string.IsNullOrEmpty(user.Organization.StripeAccountId)))
                claims.Add(new Claim("IsNotSubscribed", string.Empty));
            else if (user.Role == UserRole.Bruger && (user.Organization is not null && user.Organization.IsActive && !string.IsNullOrEmpty(user.Organization.StripeAccountId)))
                claims.Add(new Claim("StripeAccountId", user.Organization.StripeAccountId));

            if (user.Role == UserRole.Creator)
            {
                Creator? creator = await Context.Creators.SingleOrDefaultAsync(x => x.UserId == user.Id);
                if (creator is not null)
                {
                    if (string.IsNullOrEmpty(creator.StripeAccountId))
                        claims.Add(new Claim("IsNotStripeConnected", string.Empty));
                    else
                    {
                        claims.Add(new Claim("StripeAccountId", creator.StripeAccountId));

                        // Display onboarding incomplete
                        if (!await StripeService.CheckOnboardingStatus(creator.StripeAccountId))
                        {
                            claims.Add(new Claim("OnboardingIncomplete", string.Empty));
                        }
                    }

                    claims.Add(new Claim("SubType", creator.SubType.ToString()));
                    claims.Add(new Claim("CreatorId", creator.Id.ToString()));
                }
            }

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddHours(8)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
            return Redirect("/");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }
    }
}
