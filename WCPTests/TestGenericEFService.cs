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
    public class TestGenericEFService
    {
        private GenericEFService<Language> _langService;

        [TestInitialize]
        public void Init()
        {
            // Set up new DbContextOptions with an InMemory database.
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a new in-memory database for each test
                .Options;

            IWcpDbContext context = new TestDbContext(options);
            _langService = new GenericEFService<Language>(context);
        }

        [TestMethod]
        public async Task TestAdd()
        {
            var obj = new Language() { Name = "Dansk" };

            var resultDto = await _langService.AddObject(obj);
            Assert.IsNotNull(resultDto);
            Assert.AreEqual(obj.Name, resultDto.Name);
        }

        [TestMethod]
        public async Task TestGet()
        {
            var result = await _langService.AddObject(new Language() { Name = "Dansk" });

            var langs = await _langService.GetAllObjects();

            Assert.IsNotNull(langs);
            Assert.IsTrue(langs.Any());
            Assert.AreEqual(langs.Count, 1);

            var lang = await _langService.GetObject(result.Id);
            Assert.IsNotNull(lang);
            Assert.AreEqual(result.Name, lang.Name);
        }

        [TestMethod]
        public async Task TestUpdate()
        {
            var addResult = await _langService.AddObject(new Language() { Name = "Dansk" });

            Assert.AreEqual(1, (await _langService.GetAllObjects()).Count);

            var existing = await _langService.GetObject(addResult.Id);
            Assert.IsNotNull(existing);

            string updatedName = "Svensk";
            existing.Name = updatedName;

            var updatedResult = await _langService.UpdateObject(addResult.Id, existing);
            Assert.IsNotNull(updatedResult);
            Assert.AreEqual(updatedName, updatedResult.Name);

            Assert.AreEqual(1, (await _langService.GetAllObjects()).Count);

            var updatedGet = await _langService.GetObject(addResult.Id);
            Assert.IsNotNull(updatedGet);
            Assert.AreEqual(updatedName, updatedGet.Name);
        }

        [TestMethod]
        public async Task TestDelete()
        {
            var result = await _langService.AddObject(new Language()
            {
                Name = "Dansk"
            });

            Assert.IsNotNull(await _langService.GetObject(result.Id));

            var deleted = await _langService.DeleteObject(result.Id);
            Assert.IsNotNull(deleted);
            Assert.AreEqual(result.Name, deleted.Name);
        }

        [TestMethod]
        public async Task TestGetObjectsBy()
        {
            await _langService.AddObject(new Language() { Name = "Dansk" });
            await _langService.AddObject(new Language() { Name = "Svensk" });
            await _langService.AddObject(new Language() { Name = "Dansk" });
            await _langService.AddObject(new Language() { Name = "Norsk" });

            Assert.AreEqual(2, (await _langService.GetObjectsBy(x => x.Name == "Dansk")).Count);
            Assert.AreEqual(1, (await _langService.GetObjectsBy(x => x.Name == "Svensk")).Count);
            Assert.AreEqual(0, (await _langService.GetObjectsBy(x => x.Name == "Nothing")).Count);
        }

        [TestMethod]
        public async Task TestGetObjectBy()
        {
            await _langService.AddObject(new Language() { Name = "Dansk" });
            await _langService.AddObject(new Language() { Name = "Svensk" });
            await _langService.AddObject(new Language() { Name = "Dansk" });
            await _langService.AddObject(new Language() { Name = "Norsk" });

            Assert.AreEqual(1, (await _langService.GetObjectBy(x => x.Name == "Dansk"))!.Id);
            Assert.AreEqual("Dansk", (await _langService.GetObjectBy(x => x.Id == 1))!.Name);
        }
    }
}
