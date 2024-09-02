using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Interfaces;
using WCPShared.Interfaces.Auth;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models;
using WCPShared.Models.AuthModels;
using WCPShared.Models.UserModels;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Services
{
    public class JwtService : IJwtService, IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;
        private readonly IEmailService _emailService;

        public JwtService(IConfiguration configuration, IUserService userService, IEmailService emailService, IOrganizationService organizationService)
        {
            _configuration = configuration;
            _userService = userService;
            _emailService = emailService;
            _organizationService = organizationService;
        }

        public async Task<User> Register(RegisterDto request)
        {
            if (await _userService.GetUserByEmail(request.Email) is not null)
                return null!;

            Organization? org = null;
            if (request.OrganizationId != 0 && request.OrganizationId is not null)
                org = await _organizationService.GetObject(request.OrganizationId!.Value);

            string generatedPassword = GenerateRandomString(32);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(generatedPassword);
            string verificationToken = GenerateRandomString(64);

            User user = new User()
            {
                Email = request.Email,
                Name = request.Name,
                PasswordHash = passwordHash,
                Phone = request.Phone,
                Role = request.Role,
                IsActive = false,
                VerificationToken = verificationToken,
                OrganizationId = request.OrganizationId,
                Organization = org
            };                

            await _userService.AddObject(user);
            await _emailService.SendRegistrationEmail(user, verificationToken);

            return user;
        }

        public async Task<AuthResponse?> Login(UserDto request)
        {
            User? user = await _userService.GetUserByEmail(request.Email);

            if (user is null) return null;

            if (user.Email.ToLower() != request.Email.ToLower())
            {
                await LoginAttempt(false, user);
                return null;
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                await LoginAttempt(false, user);
                return null;
            }

            var refreshToken = GenerateRandomString(64);
            await SetRefreshToken(user.Id, refreshToken);
            string token = CreateToken(user); // Create JWT

            await LoginAttempt(true, user);

            return new AuthResponse
            {
                Token = token,
                RefreshToken = refreshToken
            };
        }

        public async Task<bool> AddAdmin(int id)
        {
            User? user = await _userService.GetObject(id);
            if (user is null)
                return false;

            if (user.Role != "Admin")
                return false;

            user.Role = "Admin";
            await _userService.UpdateObject(id, user);
            return true;
        }

        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.UserData, $"{user.Id}"),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            if (user.Role == "Bruger" && user.Organization is not null)
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
            User? user = await _userService.GetObject(userId);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry < DateTime.Now)
                return null;

            return CreateToken(user);
        }

        public async Task<string?> RefreshToken(string email, string refreshToken)
        {
            User? user = await _userService.GetUserByEmail(email);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry < DateTime.Now)
                return null;

            return CreateToken(user);
        }

        private async Task SetRefreshToken(int userId, string token)
        {
            User? user = await _userService.GetObject(userId);

            if (user != null)
            {
                user.RefreshToken = token;
                user.RefreshTokenExpiry = DateTime.Now.AddDays(1);
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

        public async Task LoginAttempt(bool success, User user)
        {
            if (success)
                user.LoginTries = 0;
            else if (user.LoginTries < 3)
                user.LoginTries += 1;

            user.LastLoginAttempt = DateTime.Now;
            await _userService.UpdateObject(user.Id, user);
        }

        public async Task<bool> CheckLoginAttempts(UserDto request)
        {
            User? user = await _userService.GetUserByEmail(request.Email);

            if (user is null) return false;

            // Check login attempts
            if (user.LoginTries > 0 && user.LastLoginAttempt is not null)
                if (user.LastLoginAttempt.Value.AddMinutes(15) > DateTime.Now && user.LoginTries >= 3)
                    return false;
                else if (user.LastLoginAttempt.Value.AddMinutes(15) < DateTime.Now)
                    await LoginAttempt(true, user); // Reset login tries after the 15 minutes

            return true;
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
