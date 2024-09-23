using Microsoft.EntityFrameworkCore;
using WCPShared.Interfaces.DataServices;
using WCPShared.Interfaces;
using WCPShared.Models.DTOs;
using WCPShared.Models.UserModels;
using WCPShared.Models;
using WCPShared.Services.Converters;
using WCPShared.Services.EntityFramework;
using WCPShared.Services;
using Microsoft.Extensions.Configuration;
using Azure.Core;
using WCPAuthAPI.Models.DTOs;
using WCPShared.Models.AuthModels;
using Amazon.Runtime.Internal.Transform;

namespace WCPTests
{
    [TestClass]
    public class TestAuthService
    {
        #region Initialize

        private IOrganizationService _organizationService;
        private ICreatorService _creatorService;
        private IBrandService _brandService;
        private IOrderService _orderService;
        private IUserService _userService;
        private JwtService _jwtService;
        private IEmailService _emailService;
        private ILanguageService _languageService;
        private ViewConverter _viewConverter;

        private Organization _organization;

        [TestInitialize]
        public async Task Init()
        {
            // Set up new DbContextOptions with an InMemory database.
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a new in-memory database for each test
                .Options;

            var myConfiguration = new Dictionary<string, string>
            {
                {"Jwt:GeneratedToken", "123cGj4JCipLuwjdusq4Q1pUDsg6vrXHuhkjG7testfoygQqKDJYdyHvAttlFw5Eg"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            IWcpDbContext context = new TestDbContext(options);
            _viewConverter = new ViewConverter();
            _organizationService = new OrganizationService(context, _viewConverter);
            _brandService = new BrandService(context, _organizationService, _viewConverter);
            _languageService = new LanguageService(context);
            _emailService = new SendGridEmailService(configuration);
            _userService = new UserService(context, _organizationService, _viewConverter);
            _creatorService = new CreatorService(context, _languageService, _userService, _viewConverter);
            _jwtService = new JwtService(configuration, _userService, _emailService, _organizationService, _creatorService);
            

            _organization = await _organizationService.AddObject(new OrganizationDto() { Name = "Org", CVR = "12345678" });
        }

        #endregion

        [TestMethod]
        public async Task TestRegisterUser()
        {
            RegisterDto user = new RegisterDto()
            {
                Email = "info@webcontent.dk",
                Name = "Test Bruger",
                OrganizationId = _organization!.Id,
                Phone = "12345678",
                Role = "Bruger"
            };
            
            RegisterCreatorDto dto = new RegisterCreatorDto()
            {
                User = user,
            };

            var result = await _jwtService.Register(dto);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Email, user.Email);
            Assert.IsNotNull(result.Organization);

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _jwtService.Register(dto));
        }

        [TestMethod]
        public async Task TestRegisterCreator()
        {
            RegisterCreatorDto dto = new RegisterCreatorDto()
            {
                User = new RegisterDto()
                {
                    Email = "info@creator.dk",
                    Name = "Test Creator",
                    Phone = "12345678",
                    Role = "Creator"
                },
                Creator = new CreatorDto()
                {
                    Address = "Adressevej 3, 4000 By",
                    Gender = "Mand",
                    SubType = "UGC",
                }
            };

            var result = await _jwtService.Register(dto);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Email, dto.User.Email);

            Creator? creator = (await _creatorService.GetAllObjects()).Where(x => x.UserId == result.Id).FirstOrDefault();
            Assert.IsNotNull(creator);

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _jwtService.Register(dto));
        }

        [TestMethod]
        public async Task TestRegisterCreatorFail()
        {
            RegisterCreatorDto dto = new RegisterCreatorDto()
            {
                User = new RegisterDto()
                {
                    Email = "info@creator.dk",
                    Name = "Test Creator",
                    Phone = "12345678",
                    Role = "Creator"
                },
                Creator = new CreatorDto()
                {
                    Address = "Adressevej 3, 4000 By",
                    Gender = "Mand",
                    SubType = string.Empty, // Should be initialized
                }
            };

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _jwtService.Register(dto));
        }

        [TestMethod]
        public async Task TestVerify()
        {
            RegisterCreatorDto dto = new RegisterCreatorDto()
            {
                User = new RegisterDto()
                {
                    Email = "info@webcontent.dk",
                    Name = "Test Bruger",
                    OrganizationId = _organization!.Id,
                    Phone = "12345678",
                    Role = "Bruger"
                }
            };

            var result = await _jwtService.Register(dto);
            Assert.IsNotNull(result.VerificationToken);

            VerifyUserDTO request = new VerifyUserDTO()
            {
                VerificationToken = result.VerificationToken,
                Password = "Password"
            };

            var user = await _userService.GetUserByVerificationToken(request.VerificationToken);
            Assert.IsNotNull(user);

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.PasswordHash = passwordHash;
            user.IsActive = true;

            var updateResult = await _userService.UpdateObject(user.Id, user);
            Assert.IsNotNull(updateResult);
            
            user = await _userService.GetObject(user.Id);
            Assert.IsNotNull(user);
            Assert.IsTrue(user.IsActive);
        }

        [TestMethod]
        public async Task TestLogin()
        {
            RegisterCreatorDto dto = new RegisterCreatorDto()
            {
                User = new RegisterDto()
                {
                    Email = "info@webcontent.dk",
                    Name = "Test Bruger",
                    OrganizationId = _organization!.Id,
                    Phone = "12345678",
                    Role = "Bruger"
                }
            };

            var registration = await _jwtService.Register(dto);

            ArgumentException inactiveException = await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _jwtService.Login(new UserDto
            {
                Email = dto.User.Email,
                Password = "Doesnt matter"
            }));

            VerifyUserDTO request = new VerifyUserDTO()
            {
                VerificationToken = registration.VerificationToken,
                Password = "Password"
            };

            var user = await _userService.GetUserByVerificationToken(request.VerificationToken);
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.PasswordHash = passwordHash;
            user.IsActive = true;
            await _userService.UpdateObject(user.Id, user);

            var result = await _jwtService.Login(new UserDto
            {
                Email = dto.User.Email,
                Password = request.Password
            });

            Assert.IsNotNull(result);

            ArgumentException exception = await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _jwtService.Login(new UserDto
            {
                Email = "Does@not.exist",
                Password = "Something wrong"
            }));
            Assert.AreEqual("User not found", exception.Message);

            Assert.IsNull(await _jwtService.Login(new UserDto()
            {
                Email = dto.User.Email,
                Password = "Wrong password"
            }));
        }
    }
}
