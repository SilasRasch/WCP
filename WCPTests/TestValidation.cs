using WCPShared.Models.DTOs;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.AuthModels;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Models.Enums;
using WCPShared.Services.StaticHelpers;

namespace WCPTests
{
    [TestClass]
    public class TestValidation
    {
        [TestMethod]
        public void TestValidateEmail()
        {
            string legal = "info@webcontent.dk";
            string illegal = "@illegal.email";

            Assert.IsTrue(Validation.ValidateEmail(legal));
            Assert.IsFalse(Validation.ValidateEmail(illegal));
        }

        [TestMethod]
        public void TestValidateName()
        {
            string legal = "Mathias Hansen";
            string illegal = "x";

            Assert.IsTrue(Validation.ValidateDisplayName(legal));
            Assert.IsFalse(Validation.ValidateDisplayName(illegal));
        }

        [TestMethod]
        public void TestValidateCVR()
        {
            string legal = "12345678";
            string illegalCharacter = "123a456b";
            string illegalTooLong = "12345678910";
            string illegalTooShort = "1234567";

            Assert.IsTrue(Validation.ValidateCVR(legal));
            Assert.IsFalse(Validation.ValidateCVR(illegalCharacter));
            Assert.IsFalse(Validation.ValidateCVR(illegalTooLong));
            Assert.IsFalse(Validation.ValidateCVR(illegalTooShort));
        }

        [TestMethod]
        public void TestValidatePhone()
        {
            string legalLowerLimit = "12345678";
            string legalUpperLimit = "12345678911";
            string illegalCharacter = "123b12345";
            string illegalTooLong = "123456789112";
            string illegalTooShort = "1234567";

            Assert.IsTrue(Validation.ValidatePhone(legalLowerLimit));
            Assert.IsTrue(Validation.ValidatePhone(legalUpperLimit));
            Assert.IsFalse(Validation.ValidatePhone(illegalCharacter));
            Assert.IsFalse(Validation.ValidatePhone(illegalTooShort));
            Assert.IsFalse(Validation.ValidatePhone(illegalTooLong));
        }

        [TestMethod]
        public void TestValidateBrandURL()
        {
            string legal = "webcontent.dk";
            string illegal = ".webcontent.";

            Assert.IsTrue(Validation.ValidateBrandURL(legal));
            Assert.IsFalse(Validation.ValidateBrandURL(illegal));
        }

        [TestMethod]
        public void TestValidateBrand()
        {
            Brand legal = new Brand()
            {
                Name = "WebContent",
                URL = "WebContent.dk"
            };

            Assert.IsTrue(legal.Validate());

            Brand illegal = new Brand()
            {
                Name = "",
                URL = "WebContent"
            };

            Assert.IsFalse(illegal.Validate());
        }

        [TestMethod]
        public void TestValidateBrandDto()
        {
            BrandDto legal = new BrandDto()
            {
                Name = "WebContent",
                URL = "WebContent.dk"
            };

            Assert.IsTrue(legal.Validate());

            BrandDto illegal = new BrandDto()
            {
                Name = "",
                URL = "WebContent"
            };

            Assert.IsFalse(illegal.Validate());
        }

        [TestMethod]
        public void TestValidateOrder()
        {
            Order legal = new Order()
            {
                Brand = new Brand() { Name = "WebContent.dk", URL = "WebContent.dk" },
                BrandId = 1,
                Email = "info@webcontent.dk",
                Phone = "22255123",
                Name = "Mathias Hansen",
                ProjectName = "My Project",
                ProjectType = ProjectType.UGC,
                Platforms = "TikTok, Instagram",
                Format = "4:5, 1:1"
            };

            Assert.IsTrue(legal.Validate());

            Order illegal = new Order()
            {
                Brand = new Brand() { Name = "WebContent.dk", URL = "WebContent.dk" },
                BrandId = 0,
                Email = "info@webcontent.dk",
                Phone = "22255123",
                Name = "Mathias Hansen",
                ProjectName = "My Project",
                ProjectType = ProjectType.UGC,
                Platforms = "TikTok, Instagram"
            };

            Assert.IsFalse(illegal.Validate());
        }

        [TestMethod]
        public void TestValidateOrderDto()
        {
            OrderDto legal = new OrderDto()
            {
                BrandId = 1,
                Email = "info@webcontent.dk",
                Phone = "22255123",
                Name = "Mathias Hansen",
                ProjectName = "My Project",
                ProjectType = "User Generated Content",
                Platforms = "TikTok, Instagram",
                Format = "4:5, 1:1"
            };

            Assert.IsTrue(legal.Validate());

            OrderDto illegal = new OrderDto()
            {
                BrandId = 0,
                Email = "info@webcontent.dk",
                Phone = "22255123",
                Name = "Mathias Hansen",
                ProjectName = "My Project",
                ProjectType = "User Generated Content",
                Platforms = "TikTok, Instagram"
            };

            Assert.IsFalse(illegal.Validate());
        }

        [TestMethod]
        public void TestValidateUser()
        {
            User legal = new User()
            {
                Email = "info@webcontent.dk",
                Role = UserRole.Bruger,
                Phone = "12341234",
                Name = "Mathias Hansen",
                Organization = new Organization { Name = "Org", CVR = "12341234" }
            };

            Assert.IsTrue(legal.Validate());

            User illegal = new User()
            {
                Email = "info@webcontent", // Not an email
                Phone = "12341234",
                Name = "Mathias Hansen",
                Organization = new Organization { Name = "Org", CVR = "12341234" }
            };

            Assert.IsFalse(illegal.Validate());

            illegal.Email = "info@webcontent.dk";
            illegal.Organization = null;
            Assert.IsFalse(illegal.Validate());
        }

        [TestMethod]
        public void TestValidateRegisterDto()
        {
            RegisterDto legal = new RegisterDto()
            {
                Email = "info@webcontent.dk",
                Role = "Bruger",
                Phone = "12341234",
                Name = "Mathias Hansen"
            };

            Assert.IsTrue(legal.Validate());

            RegisterDto illegal = new RegisterDto()
            {
                Email = "info@webcontent.dk",
                Phone = "12341234",
                Name = "Mathias Hansen"
            };

            Assert.IsFalse(illegal.Validate());
        }

        [TestMethod]
        public void TestValidateCreator()
        {
            Creator legal = new Creator()
            {
                SubType = CreatorSubType.UGC,
                Address = "Kirkevej 43, 4000 Roskilde",
                Gender = "Mand"
            };

            Assert.IsTrue(legal.Validate());
        }

        [TestMethod]
        public void TestValidateCreatorDto()
        {
            CreatorDto legal = new CreatorDto()
            {
                SubType = "UGC",
                Address = "Kirkevej 43, 4000 Roskilde",
                Gender = "Mand"
            };

            Assert.IsTrue(legal.Validate());

            CreatorDto illegal = new CreatorDto()
            {
                SubType = string.Empty,
            };

            Assert.IsFalse(illegal.Validate());
        }
    }
}