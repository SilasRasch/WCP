using MongoDB.Driver;
using System.Linq.Expressions;
using WCPShared.Interfaces.Mongo;
using WCPShared.Models;
using WCPShared.Models.UserModels.CreatorModels;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Services.Databases.Mongo
{
    public class CreatorService : ICreatorService
    {
        private readonly IMongoCollection<CreatorMongo> _creators;

        public CreatorService(MongoDbContext mongoDbService)
        {
            _creators = mongoDbService.Database?.GetCollection<CreatorMongo>(Secrets.MongoCreatorCollectionName)!;
        }

        public async Task AddObject(CreatorMongo obj)
        {
            if (!obj.Validate())
                return;

            await _creators.InsertOneAsync(obj);
        }

        public async Task<CreatorMongo?> DeleteObject(int id)
        {
            CreatorMongo? creator = await GetObject(id);

            if (creator is null)
                return null;

            DeleteResult result = await _creators.DeleteOneAsync(x => x.Id == id);

            if (!result.IsAcknowledged)
                return null;

            return result.DeletedCount == 1 ? creator : null;
        }

        public async Task<IEnumerable<CreatorMongo>> GetAllObjects()
        {
            return await _creators.FindAsync(FilterDefinition<CreatorMongo>.Empty).Result.ToListAsync();
        }

        public async Task<IEnumerable<CreatorMongo>> GetAllObjects(Expression<Func<CreatorMongo, bool>>? filter = null)
        {
            return await _creators.FindAsync(filter).Result.ToListAsync();
        }

        public async Task<CreatorMongo?> GetObject(int id)
        {
            return await _creators.FindAsync(x => x.Id == id).Result.FirstOrDefaultAsync();
        }

        public async Task<CreatorMongo?> UpdateObject(int id, CreatorMongo obj)
        {
            CreatorMongo? oldCreator = await GetObject(id);
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
