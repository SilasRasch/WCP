using MongoDB.Driver;
using System.Linq.Expressions;
using WCPShared.Interfaces;
using WCPShared.Models;
using WCPShared.Models.UserModels.CreatorModels;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Services.Databases
{
    public class CreatorService : ICreatorService
    {
        private readonly IMongoCollection<Creator> _creators;

        public CreatorService(MongoDbContext mongoDbService)
        {
            _creators = mongoDbService.Database?.GetCollection<Creator>(Secrets.MongoCreatorCollectionName)!;
        }

        public async Task AddObject(Creator obj)
        {
            if (!obj.Validate())
                return;

            await _creators.InsertOneAsync(obj);
        }

        public async Task<Creator?> DeleteObject(int id)
        {
            Creator? creator = await _creators.FindAsync(x => x.Id == id).Result.FirstOrDefaultAsync();

            if (creator is null)
                return null;

            DeleteResult result = await _creators.DeleteOneAsync(x => x.Id == id);

            if (!result.IsAcknowledged)
                return null;

            return result.DeletedCount == 1 ? creator : null;
        }

        public async Task<IEnumerable<Creator>> GetAllObjects()
        {
            return await _creators.FindAsync(FilterDefinition<Creator>.Empty).Result.ToListAsync();
        }

        public async Task<IEnumerable<Creator>> GetAllObjects(Expression<Func<Creator, bool>>? filter = null)
        {
            return await _creators.FindAsync(filter).Result.ToListAsync();
        }

        public async Task<Creator?> GetObject(int id)
        {
            return await _creators.FindAsync(x => x.Id == id).Result.FirstOrDefaultAsync();
        }

        public async Task<Creator?> UpdateObject(int id, Creator obj)
        {
            Creator? oldCreator = await _creators.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (oldCreator is null)
                return null;

            if (!obj.Validate())
                throw new Exception("Validation error");

            if (id != obj.Id)
                throw new Exception("Ids must match in URI and body");

            ReplaceOneResult result = await _creators.ReplaceOneAsync(x => x.Id == obj.Id, obj);

            if (result.IsAcknowledged)
                return result.ModifiedCount == 1 ? obj : throw new Exception("Error in database");

            return null;
        }
    }
}
