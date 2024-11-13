using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Errors.Model;
using System.Security.Cryptography;
using WCPShared.Interfaces;
using WCPShared.Interfaces.Auth;
using WCPShared.Services.EntityFramework;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.AuthModels;
using WCPShared.Models.Entities.UserModels;
using Microsoft.EntityFrameworkCore;
using WCPShared.Models.Views;
using WCPShared.Models.Enums;
using WCPShared.Migrations.TestDb;

namespace WCPShared.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly LanguageService _languageService;
        private readonly IEmailService _emailService;
        private readonly UserService _userService;
        private readonly OrganizationService _organizationService;
        private readonly CreatorService _creatorService;
        private readonly UserContextService _userContextService;
        private readonly IJwtService _jwtService;
        private readonly IWcpDbContext _context;

        public AuthService(IConfiguration configuration, IWcpDbContext context, UserService userService, IEmailService emailService, OrganizationService organizationService, CreatorService creatorService, UserContextService userContextService, IJwtService jwtService, LanguageService languageService)
        {
            _configuration = configuration;
            _userService = userService;
            _emailService = emailService;
            _organizationService = organizationService;
            _creatorService = creatorService;
            _userContextService = userContextService;
            _jwtService = jwtService;
            _languageService = languageService;
            _context = context;
        }

        public async Task<User?> Register(RegisterCreatorDto request, bool selfRegister = false)
        {
            if (await _userService.GetObjectBy(x => x.Email == request.User.Email) is not null)
                throw new ArgumentException("A user with that email already exists...");

            Organization? org = null;
            if (request.User.OrganizationId != 0 && request.User.OrganizationId is not null)
                org = await _organizationService.GetObject(request.User.OrganizationId!.Value);

            var language = await _languageService.GetObject(request.User.LanguageId);

            string generatedPassword = GenerateRandomString(32);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(generatedPassword);
            string verificationToken = GenerateRandomString(64);

            User user = new User()
            {
                Email = request.User.Email,
                Name = request.User.Name,
                PasswordHash = passwordHash,
                Phone = request.User.Phone,
                Role = (UserRole)Enum.Parse(typeof(UserRole), request.User.Role),
                IsActive = false,
                VerificationToken = verificationToken,
                OrganizationId = request.User.OrganizationId,
                Organization = org,
                NotificationsOn = true,
                NotificationSetting = "slack"
            };

            if (language is not null)
            {
                user.Language = language;
                user.LanguageId = language.Id;
            }
                

            user = await _userService.AddObject(user);

            if (request.Creator is not null && user is not null && user.Id != 0 && request.User.Role == "Creator")
            {
                request.Creator.UserId = user.Id;
                Creator? creator = await _creatorService.AddObject(request.Creator);

                if (creator is null || creator.Id == 0)
                {
                    await _userService.DeleteObject(user.Id);
                    throw new ArgumentException("Adding creator failed...");
                }
            }

            if (user is not null)
                try { await _emailService.SendRegistrationEmail(user, verificationToken, selfRegister); } 
                catch { /* ignore */ }

            return user;
        }

        public async Task<User?> Register(User user, Creator? creator = null, bool selfRegister = false)
        {
            if (!user.Validate())
                throw new ArgumentException("Validation error");
            
            if (await _userService.GetObjectBy(x => x.Email == user.Email) is not null)
                throw new ArgumentException("A user with that email already exists...");

            string generatedPassword = GenerateRandomString(32);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(generatedPassword);
            string verificationToken = GenerateRandomString(64);

            user.PasswordHash = passwordHash;
            user.VerificationToken = verificationToken;
            user.NotificationsOn = true;
            user.NotificationSetting = "slack";

            user = await _userService.AddObject(user);

            if (creator is not null && user is not null && user.Id != 0 && user.Role == UserRole.Creator)
            {
                if (!creator.Validate())
                    throw new ArgumentException("Validation error");

                creator.User = user;
                creator = await _creatorService.AddObject(creator);

                if (creator is null || creator.Id == 0)
                {
                    await _userService.DeleteObject(user.Id);
                    throw new ArgumentException("Adding creator failed...");
                }
            }

            if (user is not null)
                try { await _emailService.SendRegistrationEmail(user, verificationToken, selfRegister); }
                catch { /* ignore */ }

            return user;
        }

        public async Task<AuthResponse?> Login(UserDto request)
        {
            User? user = await _context.Users
                .Include(x => x.Organization)
                .SingleOrDefaultAsync(x => x.Email == request.Email);

            if (user is null) throw new NotFoundException("User not found");
            if (!user.IsActive) throw new ArgumentException("User deactivated");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                await LoginAttempt(false, user);
                return null;
            }

            var refreshToken = GenerateRandomString(64);
            await _jwtService.SetRefreshToken(user.Id, refreshToken);
            string token = _jwtService.CreateToken(user); // Create JWT

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

            if (user.Role != UserRole.Admin)
                return false;

            user.Role = UserRole.Admin;
            await _userService.UpdateObject(id, user);
            return true;
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
            User? user = await _userService.GetObjectBy(x => x.Email == request.Email);

            if (user is null) return false;

            // Check login attempts
            if (user.LoginTries > 0 && user.LastLoginAttempt is not null)
                if (user.LastLoginAttempt.Value.AddMinutes(15) > DateTime.Now && user.LoginTries >= 3)
                    return false;
                else if (user.LastLoginAttempt.Value.AddMinutes(15) < DateTime.Now)
                    await LoginAttempt(true, user); // Reset login tries after the 15 minutes

            return true;
        }

        public async Task<User?> SelfRegister(SelfRegisterDto request)
        {
            var user = await _userService.GetObjectBy(x => x.VerificationToken == request.VerificationToken);
            if (user == null) throw new NotFoundException("User not found");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.PasswordHash = passwordHash;
            user.Phone = request.User.Phone;
            user.Name = request.User.Name;
            user.IsActive = true;

            if (!user.Validate()) throw new ArgumentException("Validation failed...");

            var updateResult = await _userService.UpdateObject(user.Id, user);
            if (updateResult is null) throw new ArgumentException("Updating user failed...");

            if (user.Role == UserRole.Creator && request.Creator is not null)
            {
                if (!request.Creator.Validate()) throw new ArgumentException("Creator validation failed");
                var creator = await _creatorService.GetObjectBy(x => x.UserId == user.Id);
                if (creator == null) throw new NotFoundException("Creator not found");

                request.Creator.SubType = creator.SubType.ToString();
                var result = await _creatorService.UpdateObject(creator.Id, request.Creator);
                if (result is null) throw new ArgumentException("Something went wrong...");
            }

            user.VerificationToken = null;
            updateResult = await _userService.UpdateObject(user.Id, user);

            return updateResult is not null ? updateResult : throw new ArgumentException("Verification token removal failed");
        }

        public async Task<User?> Verify(VerifyUserDto request)
        {
            var user = await _userService.GetObjectBy(x => x.VerificationToken == request.VerificationToken);
            if (user == null) throw new NotFoundException();

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.PasswordHash = passwordHash;
            user.IsActive = true;

            var result = await _userService.UpdateObject(user.Id, user);
            return result;
        }

        public async Task<dynamic> Authenticate()
        {
            string email = _userContextService.GetEmail();
            User? user = await _context.Users.Include(x => x.Organization).SingleOrDefaultAsync(x => x.Email == email);

            if (user is null) throw new NotFoundException("No user with the given email");

            var id = user.Id;
            var roles = _userContextService.GetRoles();
            var name = user.Name;
            var phone = user.Phone;
            var notificationSetting = user.NotificationSetting;

            if (user.Role == UserRole.Bruger && user.Organization is not null)
                return new { id, orgId = user.Organization.Id, email, roles, name, notificationSetting, phone };

            if (user.Role == UserRole.Creator)
            {
                CreatorView? creator = await _creatorService.GetObjectViewBy(x => x.UserId == _userContextService.GetId());
                if (creator is not null)
                    return new { id, creatorId = creator.Id, subType = creator.SubType, email, roles, name, notificationSetting, phone };
            }

            return new { id, email, roles, name, notificationSetting, phone };
        }
    }
}
