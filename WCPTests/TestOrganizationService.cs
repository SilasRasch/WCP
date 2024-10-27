using Microsoft.EntityFrameworkCore;
using WCPShared.Interfaces;
using WCPShared.Models;
using WCPShared.Models.DTOs;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Services.Converters;
using WCPShared.Services.EntityFramework;

namespace WCPTests
{
    [TestClass]
    public class TestOrganizationService
    {
    private OrganizationService _organizationService;
    private ViewConverter _viewConverter;
    private LanguageService _languageService;

    [TestInitialize]
    public async Task Init()
    {
        // Set up new DbContextOptions with an InMemory database.
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a new in-memory database for each test
            .Options;

        IWcpDbContext context = new TestDbContext(options);
        _viewConverter = new ViewConverter();
        _languageService = new LanguageService(context);
        _organizationService = new OrganizationService(context, _viewConverter, _languageService);

        await _languageService.AddObject(new Language()
        {
            Name = "DAN",
        });
    }
        
    [TestMethod]
    public async Task TestAdd()
    {
        var dto = new OrganizationDto()
        {
            Name = "First Org",
            CVR = "12341234",
            LanguageId = 1,
        };

        var resultDto = await _organizationService.AddObject(dto);
        Assert.IsNotNull(resultDto);
        Assert.AreEqual(dto.Name, resultDto.Name);
    }

    [TestMethod]
    public async Task TestGet()
    {
        var result = await _organizationService.AddObject(new OrganizationDto()
        {
            Name = "First Org",
            CVR = "12341234",
            LanguageId = 1,
        });

        var orgs = await _organizationService.GetAllObjects();

        Assert.IsNotNull(orgs);
        Assert.IsTrue(orgs.Any());
        Assert.AreEqual(orgs.Count, 1);

        var org = await _organizationService.GetObject(result.Id);
        Assert.IsNotNull(org);
        Assert.AreEqual(result.Name, org.Name);
    }

    [TestMethod]
    public async Task TestUpdate()
    {
        Organization? addResult = await _organizationService.AddObject(new OrganizationDto()
        {
            Name = "First Org",
            CVR = "12341234",
            LanguageId = 1,
        });

        Assert.AreEqual(1, (await _organizationService.GetAllObjects()).Count);

        var existing = await _organizationService.GetObject(addResult.Id);
        Assert.IsNotNull(existing);

        string updatedName = "Second Org";
        existing.Name = updatedName;

        Organization? updatedResult = await _organizationService.UpdateObject(addResult.Id, existing);
        Assert.IsNotNull(updatedResult);
        Assert.AreEqual(updatedName, updatedResult.Name);

        Assert.AreEqual(1, (await _organizationService.GetAllObjects()).Count);

        Organization? updatedGet = await _organizationService.GetObject(addResult.Id);
        Assert.IsNotNull(updatedGet);
        Assert.AreEqual(updatedName, updatedGet.Name);
    }

    [TestMethod]
    public async Task TestDelete()
    {
        var result = await _organizationService.AddObject(new OrganizationDto()
        {
            Name = "First Org",
            CVR = "12341234",
            LanguageId = 1,
        });

        Assert.IsNotNull(await _organizationService.GetObject(result.Id));

        var deleted = await _organizationService.DeleteObject(result.Id);
        Assert.IsNotNull(deleted);
        Assert.AreEqual(result.Name, deleted.Name);
    }
    }
}
