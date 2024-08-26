using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Services.Databases
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IConfiguration configuration)
        {
            string connectionString = Secrets.GetMongoConnectionString(configuration);
            var mongoUrl = MongoUrl.Create(connectionString);
            var mongoClient = new MongoClient(mongoUrl);
            _database = mongoClient.GetDatabase(Secrets.MongoDBName);
        }

        public IMongoDatabase? Database => _database;
    }
}
