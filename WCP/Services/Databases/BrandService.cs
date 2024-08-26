using MongoDB.Driver;
using System.Linq.Expressions;
using WCPShared.Interfaces;
using WCPShared.Models;
using WCPShared.Models.BrandModels;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Services.Databases
{
    public class BrandService : IBrandService
    {
        private readonly IEmailService _emailService;
        private readonly IMongoCollection<Brand> _brands;

        public BrandService(MongoDbContext mongoDbService, IEmailService emailService)
        {
            _brands = mongoDbService.Database?.GetCollection<Brand>(Secrets.MongoBrandCollectionName)!;
            _emailService = emailService;
        }

        public async Task AddObject(Brand obj)
        {
            if (!obj.Validate())
                return;
            
            Brand lastBrand = null!;
            if (await _brands.CountDocumentsAsync(FilterDefinition<Brand>.Empty) > 0)
                lastBrand = await _brands.Find(FilterDefinition<Brand>.Empty).SortByDescending(o => o.Id).Limit(1).FirstAsync();


            obj.Id = lastBrand != null ? lastBrand.Id + 1 : 1000;

            await _brands.InsertOneAsync(obj);
            await _emailService.SendBrandCreationEmail(obj);
        }

        public async Task<Brand?> DeleteObject(int id)
        {
            Brand? brand = await _brands.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (brand is null) 
                return null;

            DeleteResult result = await _brands.DeleteOneAsync(x => x.Id == id);

            if (!result.IsAcknowledged)
                return null;

            return result.DeletedCount == 1 ? brand : null;
        }

        public async Task<IEnumerable<Brand>> GetAllObjects()
        {
            return await _brands.FindAsync(FilterDefinition<Brand>.Empty).Result.ToListAsync();
        }
        
        public async Task<IEnumerable<Brand>> GetAllObjects(Expression<Func<Brand, bool>>? filter = null)
        {
            return await _brands.FindAsync(filter).Result.ToListAsync();
        }

        public async Task<Brand?> GetObject(int id)
        {
            return await _brands.FindAsync(x => x.Id == id).Result.FirstOrDefaultAsync();
        }

        public async Task<Brand?> UpdateObject(int id, Brand obj)
        {
            Brand? oldBrand = await _brands.Find(x => x.Id == id).FirstOrDefaultAsync();
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
