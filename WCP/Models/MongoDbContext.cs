using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Models
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            string connectionString = Secrets.GetMongoConnectionString(configuration);
            var mongoUrl = MongoUrl.Create(connectionString);
            var mongoClient = new MongoClient(mongoUrl);
            _database = mongoClient.GetDatabase(Secrets.MongoDBName);
        }

        public IMongoDatabase? Database => _database;
    }
}
