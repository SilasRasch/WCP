using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WCPShared.Interfaces;
using WCPShared.Interfaces.Auth;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Services.EntityFramework;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IWcpDbContext _context;
        private readonly UserService _userService;
        private readonly OrganizationService _organizationService;
        private readonly CreatorService _creatorService;

        public JwtService(IConfiguration configuration, IWcpDbContext context, UserService userService, IEmailService emailService, OrganizationService organizationService, CreatorService creatorService)
        {
            _configuration = configuration;
            _context = context;
            _userService = userService;
            _emailService = emailService;
            _organizationService = organizationService;
            _creatorService = creatorService;
        }

        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.UserData, $"{user.Id}"),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            if (user.Role == Models.Enums.UserRole.Bruger && user.Organization is not null)
                claims.Add(new Claim("OrganizationId", user.Organization!.Id.ToString()));

            if (user.Phone is not null)
                claims.Add(new Claim(ClaimTypes.MobilePhone, user.Phone));

            var key = Secrets.GetJwtKey(_configuration);
            var keyBytes = Encoding.UTF8.GetBytes(key);

            // The private key and hashing-algorithm used to create the signature for verifying authenticity
            var _key = new SymmetricSecurityKey(keyBytes); // Private key used to encrypt, decrypt, and sign JWT
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials,
                issuer: Secrets.Issuer,
                audience: Secrets.Audience
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<string?> RefreshToken(int userId, string refreshToken)
        {
            //User? user = await _context.Users.Include(x => x.Organization).SingleOrDefaultAsync(x => x.Id == userId);

            //if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry < DateTime.Now)
            //    return null;

            //return CreateToken(user);
            throw new NotImplementedException();
        }

        public async Task<string?> RefreshToken(string email, string refreshToken)
        {
            //User? user = await _context.Users.Include(x => x.Organization).SingleOrDefaultAsync(x => x.Email == email);

            //if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry < DateTime.Now)
            //    return null;

            //return CreateToken(user);
            throw new NotImplementedException();
        }

        public async Task SetRefreshToken(int userId, string token)
        {
            User? user = await _userService.GetObject(userId);

            if (user != null)
            {
                //user.RefreshToken = token;
                //user.RefreshTokenExpiry = DateTime.Now.AddDays(1);
                await _userService.UpdateObject(userId, user);
            }
        }

        public async Task RevokeSession(int userId)
        {
            await SetRefreshToken(userId, "revoked");
        }

        public string GenerateRandomString(int byteCount)
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(byteCount));
        }

        public async Task<bool> ValidateToken(string token)
        {
            if (String.IsNullOrEmpty(token))
                return false;
            
            var jwt = await new JwtSecurityTokenHandler().ValidateTokenAsync(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = Secrets.Issuer,
                ValidAudiences = Secrets.GetAudiences(),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secrets.GetJwtKey(_configuration)))
            });

            return jwt.IsValid;
        }
    }
}
