﻿using Microsoft.EntityFrameworkCore;
using WCPShared.Interfaces.DataServices;
using WCPShared.Interfaces;
using WCPShared.Models.DTOs;
using WCPShared.Models;
using WCPShared.Services.Converters;
using WCPShared.Services.EntityFramework;
using WCPShared.Services;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Models.Entities.AuthModels;
using WCPShared.Models.Enums;

namespace WCPTests
{
    [TestClass]
    public class TestOrderService
    {
        #region Initialize

        private OrganizationService _organizationService;
        private StaticTemplateService _staticTemplateService;
        private CreatorService _creatorService;
        private BrandService _brandService;
        private OrderService _orderService;
        private UserService _userService;
        private LanguageService _languageService;
        private SlackNotificationService _slackNotificationService;
        private ViewConverter _viewConverter;
        private StripeService _stripeService;

        private Organization _organization;
        private Brand _brand;
        private User _user;
        private Creator _creator;
        private Creator _creator2;
        private Creator _scripter;

        private OrderDto _orderDto = new OrderDto()
        {
            Price = 2500,
            DeliveryTimeFrom = 1,
            DeliveryTimeTo = 3,
            Status = 2,
            Scripts = "https://drive.google.com/",
            Content = "https://drive.google.com/",
            Delivery = "https://drive.google.com/",
            Other = "https://drive.google.com/",
            Creators = new List<int>([1]),
            BrandId = 1,
            Name = "Mathias Hansen",
            Email = "info@webcontent.dk",
            Phone = "12345678",
            ProjectName = "Projekt 1",
            ProjectType = "UGC",
            ContentCount = 6,
            ContentLength = 30,
            Platforms = "TikTok, Instagram",
            Format = "4:5, 1:1",
            ExtraCreator = false,
            ExtraHook = 2,
            ExtraNotes = "Noter",
            Ideas = new List<string>(["Test", "Ideer"]),
            Products = new List<string>(["Test", "Produkter"])
        };

        [TestInitialize]
        public async Task Init()
        {
            // Set up new DbContextOptions with an InMemory database.
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a new in-memory database for each test
                .Options;

            IWcpDbContext context = new TestDbContext(options);
            _viewConverter = new ViewConverter();
            _stripeService = new StripeService(context);
            _languageService = new LanguageService(context);
            _organizationService = new OrganizationService(context, _viewConverter, _languageService);
            _brandService = new BrandService(context, _organizationService, _viewConverter);
            _staticTemplateService = new StaticTemplateService(context, _viewConverter);
            _userService = new UserService(context, _organizationService, _languageService, _viewConverter);
            _creatorService = new CreatorService(context, _languageService, _userService, _viewConverter);
            _slackNotificationService = new SlackNotificationService(new SlackNet.SlackApiClient("mock"), _creatorService, _userService);
            _orderService = new OrderService(context, _brandService, _creatorService, _viewConverter, _slackNotificationService, _staticTemplateService, _stripeService);

            await _languageService.AddObject(new Language { Name = "DAN" });
            _organization = await _organizationService.AddObject(new OrganizationDto() { Name = "Org", CVR = "12345678", LanguageId = 1 });
            _brand = await _brandService.AddObject(new BrandDto() { Name = "Brand", URL = "brand.dk", OrganizationId = _organization!.Id });
            _user = await _userService.AddObject(new RegisterDto() { Name = "Creator", Email = "ugc@email.wcp", OrganizationId = _organization!.Id, Phone = "12341234", Role = "Creator", LanguageId = 1 });
            await _userService.AddObject(new RegisterDto() { Name = "Scripter", Email = "scripter@email.wcp", OrganizationId = _organization!.Id, Phone = "12341234", Role = "Creator", LanguageId = 1 });
            await _userService.AddObject(new RegisterDto() { Name = "UGC", Email = "ugc2@email.wcp", Phone = "12341234", Role = "Creator", LanguageId = 1 });
            _creator = await _creatorService.AddObject(new CreatorDto() { Address = "Adressevej 12, 4000 By", Gender = "Mand", SubType = "UGC", UserId = 1, ImgURL = "" });
            _creator2 = await _creatorService.AddObject(new CreatorDto() { Address = "Adressevej 12, 4000 By", Gender = "Mand", SubType = "UGC", UserId = 3, ImgURL = "" });
            _scripter = await _creatorService.AddObject(new CreatorDto() { Address = "Adressevej 12, 4000 By", Gender = "Mand", SubType = "Scripter", UserId = 2, ImgURL = "" });
        }

        #endregion

        [TestMethod]
        public async Task TestAdd()
        {
            var result = await _orderService.AddObject(_orderDto);
            Assert.IsNotNull(result);
            Assert.AreEqual(_orderDto.ProjectName, result.ProjectName);
            Assert.AreEqual(1, (await _orderService.GetAllObjects()).Count);
            Assert.AreEqual(1, result.Participations.Count);
            Assert.AreEqual("Brand", result.Brand.Name);
        }

        [TestMethod]
        public async Task TestGet()
        {
            await _orderService.AddObject(_orderDto);
            
            var orders = await _orderService.GetAllObjects();

            Assert.IsNotNull(orders);
            Assert.IsTrue(orders.Any());
            Assert.AreEqual(orders.Count, 1);

            var order = await _orderService.GetObject(1);
            Assert.IsNotNull(order);
            Assert.AreEqual("Projekt 1", order.ProjectName);
        }

        [TestMethod]
        public async Task TestUpdate()
        {
            Order? addResult = await _orderService.AddObject(_orderDto);

            Assert.AreEqual(1, (await _orderService.GetAllObjects()).Count);

            var existing = await _orderService.GetObject(addResult.Id);
            Assert.IsNotNull(existing);

            string updatedName = "Project 2";
            existing.ProjectName = updatedName;

            Order? updatedResult = await _orderService.UpdateObject(addResult.Id, existing);
            Assert.IsNotNull(updatedResult);
            Assert.AreEqual(updatedName, updatedResult.ProjectName);

            Assert.AreEqual(1, (await _orderService.GetAllObjects()).Count);

            Order? updatedGet = await _orderService.GetObject(addResult.Id);
            Assert.IsNotNull(updatedGet);
            Assert.AreEqual(updatedName, updatedGet.ProjectName);

            Order? nullResult = await _orderService.UpdateObject(99, existing);
            Assert.IsNull(nullResult);
        }

        [TestMethod]
        public async Task TestUpdateDto()
        {
            Order? addResult = await _orderService.AddObject(_orderDto);

            Assert.AreEqual(1, (await _orderService.GetAllObjects()).Count);

            var existing = await _orderService.GetObject(addResult!.Id);
            Assert.IsNotNull(existing);

            string updatedName = "Project 2";
            _orderDto.ProjectName = updatedName;

            Order? updatedResult = await _orderService.UpdateObject(addResult.Id, _orderDto);
            Order? nullResult = await _orderService.UpdateObject(99, _orderDto);

            Assert.IsNotNull(updatedResult);
            Assert.IsNull(nullResult);
            Assert.AreEqual(updatedName, updatedResult.ProjectName);

            Assert.AreEqual(1, (await _orderService.GetAllObjects()).Count);

            Order? updatedGet = await _orderService.GetObject(addResult.Id);
            Assert.IsNotNull(updatedGet);
            Assert.AreEqual(updatedName, updatedGet.ProjectName);
        }

        [TestMethod]
        public async Task TestDelete()
        {
            var result = await _orderService.AddObject(_orderDto);

            Assert.IsNotNull(await _orderService.GetObject(result.Id));

            var deleted = await _orderService.DeleteObject(result.Id);
            var nullResult = await _orderService.DeleteObject(99);
            Assert.IsNotNull(deleted);
            Assert.IsNull(nullResult);
            Assert.AreEqual(result.Name, deleted.Name);
        }

        [TestMethod]
        public async Task TestGetObjectBy()
        {
            await _orderService.AddObject(_orderDto);

            Order? result = await _orderService.GetObjectBy(x => x.ProjectName == _orderDto.ProjectName);
            Order? nullResult = await _orderService.GetObjectBy(x => x.ProjectName == "Does not exist");

            Assert.IsNotNull(result);
            Assert.IsNull(nullResult);
            Assert.AreEqual(_orderDto.ProjectName, result.ProjectName);
        }

        [TestMethod]
        public async Task TestGetObjectsBy()
        {
            await _orderService.AddObject(_orderDto);
            _orderDto.ProjectName = "Projekt 2";
            await _orderService.AddObject(_orderDto);

            var result = await _orderService.GetObjectsBy(x => x.ProjectName == _orderDto.ProjectName);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            Assert.AreEqual(_orderDto.ProjectName, result.First().ProjectName);
        }

        [TestMethod]
        public async Task TestObjectViews()
        {
            await _orderService.AddObject(_orderDto);

            var allViews = await _orderService.GetAllObjectsView();
            Assert.IsNotNull(allViews);
            Assert.AreEqual(1, allViews.Count);
            Assert.IsNotNull(allViews.First().Creators);
            Assert.IsNotNull(allViews.First().Brand);
            Assert.IsNotNull(allViews.First().Brand.Organization);

            var singleView = await _orderService.GetObjectViewBy(x => x.Id == 1);
            Assert.IsNotNull(singleView);
            Assert.IsNotNull(singleView.Creators);
            Assert.IsNotNull(singleView.Brand);
            Assert.IsNotNull(singleView.Brand.Organization);

            var singleViewNull = await _orderService.GetObjectViewBy(x => x.Id == 99);
            Assert.IsNull(singleViewNull);

            await _orderService.AddObject(_orderDto);
            var filteredView = await _orderService.GetObjectsViewBy(x => x.Id == 1);
            Assert.IsNotNull(filteredView);
            Assert.AreEqual(1, filteredView.Count);
            Assert.IsNotNull(filteredView.First().Creators);
            Assert.IsNotNull(filteredView.First().Brand);
            Assert.IsNotNull(filteredView.First().Brand.Organization);

            filteredView = await _orderService.GetObjectsViewBy(x => x.Status == 0); // Should return both
            Assert.IsNotNull(filteredView);
            Assert.AreEqual(2, filteredView.Count);
        }

        [TestMethod]
        public async Task TestCreatorDelivery()
        {
            var result = await _orderService.AddObject(_orderDto);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Participations.All(x => !x.HasDelivered));

            result.Status = ProjectStatus.Scripting;
            result.Participations.Add(new CreatorParticipation()
            {
                Creator = _scripter,
                CreatorId = 2,
                HasDelivered = false,
                Project = null,
                ProjectId = result.Id,
                Salary = 0,
            });
            await _orderService.UpdateObject(result.Id, result);

            await _orderService.CreatorDelivery(result, result.Participations.Last().CreatorId);
            Assert.IsTrue(result.Participations.Last().HasDelivered);
            Assert.AreEqual(ProjectStatus.Planned, result.Status);

            // Simulate customer confirming scripts
            await _orderService.UpdateObject(result.Id, result);
            result.Status = ProjectStatus.CreatorFilming;

            await _orderService.CreatorDelivery(result, result.Participations.First().CreatorId);
            Assert.IsTrue(result.Participations.First().HasDelivered);
            Assert.AreEqual(ProjectStatus.Editing, result.Status);
        }

        [TestMethod]
        public async Task TestCreatorDeliveryScripterOnly()
        {
            var result = await _orderService.AddObject(_orderDto);
            Assert.IsNotNull(result);

            result.Status = ProjectStatus.Scripting;
            result.Participations.Clear();
            result.Participations.Add(new CreatorParticipation()
            {
                Creator = _scripter,
                CreatorId = 2,
                HasDelivered = false,
                Project = null,
                ProjectId = result.Id,
                Salary = 0,
            });
            await _orderService.UpdateObject(result.Id, result);

            Assert.IsTrue(result.Participations.All(x => !x.HasDelivered));

            await _orderService.CreatorDelivery(result, result.Participations.First().CreatorId);
            Assert.IsTrue(result.Participations.First().HasDelivered);
            Assert.AreEqual(ProjectStatus.Planned, result.Status);
        }

        [TestMethod]
        public async Task TestCreatorDeliveryMultipleUgc()
        {
            var result = await _orderService.AddObject(_orderDto);
            Assert.IsNotNull(result);

            result.Participations.Add(new CreatorParticipation()
            {
                Creator = _creator2,
                CreatorId = 3,
                HasDelivered = false,
                Project = null,
                ProjectId = result.Id,
                Salary = 0,
            });
            result.Status = ProjectStatus.CreatorFilming;
            await _orderService.UpdateObject(result.Id, result);

            Assert.IsTrue(result.Participations.All(x => !x.HasDelivered));

            await _orderService.CreatorDelivery(result, result.Participations.First().CreatorId);
            Assert.IsTrue(result.Participations.First().HasDelivered);
            Assert.AreEqual(ProjectStatus.CreatorFilming, result.Status);

            await _orderService.CreatorDelivery(result, result.Participations.Last().CreatorId);
            Assert.IsTrue(result.Participations.Last().HasDelivered);
            Assert.AreEqual(ProjectStatus.Editing, result.Status);
        }
    }
}