using MongoDB.Driver;
using System.Linq.Expressions;
using WCPShared.Interfaces;
using WCPShared.Interfaces.Mongo;
using WCPShared.Models;
using WCPShared.Models.BrandModels;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Services.Databases.Mongo
{
    public class BrandService : IBrandService
    {
        private readonly IEmailService _emailService;
        private readonly IMongoCollection<BrandMongo> _brands;

        public BrandService(MongoDbContext mongoDbService, IEmailService emailService)
        {
            _brands = mongoDbService.Database?.GetCollection<BrandMongo>(Secrets.MongoBrandCollectionName)!;
            _emailService = emailService;
        }

        public async Task AddObject(BrandMongo obj)
        {
            if (!obj.Validate())
                return;

            BrandMongo lastBrand = null!;
            if (await _brands.CountDocumentsAsync(FilterDefinition<BrandMongo>.Empty) > 0)
                lastBrand = await _brands.Find(FilterDefinition<BrandMongo>.Empty).SortByDescending(o => o.Id).FirstAsync();

            obj.Id = lastBrand != null ? ++lastBrand.Id : 1000;

            await _brands.InsertOneAsync(obj);
            await _emailService.SendBrandCreationEmail(obj);
        }

        public async Task<BrandMongo?> DeleteObject(int id)
        {
            BrandMongo? brand = await GetObject(id);

            if (brand is null)
                return null;

            DeleteResult result = await _brands.DeleteOneAsync(x => x.Id == id);

            if (!result.IsAcknowledged)
                return null;

            return result.DeletedCount == 1 ? brand : null;
        }

        public async Task<IEnumerable<BrandMongo>> GetAllObjects()
        {
            return await _brands.FindAsync(FilterDefinition<BrandMongo>.Empty).Result.ToListAsync();
        }

        public async Task<IEnumerable<BrandMongo>> GetAllObjects(Expression<Func<BrandMongo, bool>>? filter = null)
        {
            return await _brands.FindAsync(filter).Result.ToListAsync();
        }

        public async Task<BrandMongo?> GetObject(int id)
        {
            return await _brands.FindAsync(x => x.Id == id).Result.FirstOrDefaultAsync();
        }

        public async Task<BrandMongo?> UpdateObject(int id, BrandMongo obj)
        {
            BrandMongo? oldBrand = await GetObject(id);
            if (oldBrand is null)
                return null;

            if (!obj.Validate())
                throw new Exception("Validation error");

            ReplaceOneResult result = await _brands.ReplaceOneAsync(x => x.Id == obj.Id, obj);

            if (result.IsAcknowledged)
                return result.ModifiedCount == 1 ? obj : throw new Exception("Error in database");

            return null;
        }
    }
}
