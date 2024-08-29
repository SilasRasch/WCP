using EllipticCurve.Utils;
using MongoDB.Driver;
using System.Linq.Expressions;
using WCPShared.Interfaces;
using WCPShared.Interfaces.Mongo;
using WCPShared.Models;
using WCPShared.Models.OrderModels;
using WCPShared.Models.UserModels.CreatorModels;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Services.Databases.Mongo
{
    public class OrderService : IOrderService
    {
        private readonly IMongoCollection<OrderMongo> _orders;
        private readonly IEmailService _emailService;

        public OrderService(MongoDbContext mongoDbService, IEmailService emailService)
        {
            _orders = mongoDbService.Database?.GetCollection<OrderMongo>(Secrets.MongoCollectionName)!;
            _emailService = emailService;
        }

        public async Task AddObject(OrderMongo obj)
        {
            if (!obj.Validate())
                return;

            OrderMongo? lastOrder = null;
            if (await _orders.CountDocumentsAsync(FilterDefinition<OrderMongo>.Empty) > 0)
                lastOrder = await _orders.Find(FilterDefinition<OrderMongo>.Empty).SortByDescending(o => o.Id).FirstAsync();

            obj.Id = lastOrder != null ? ++lastOrder.Id : 1000;

            await _orders.InsertOneAsync(obj);
        }

        public async Task<OrderMongo?> DeleteObject(int id)
        {
            OrderMongo? order = await GetObject(id);

            if (order is null)
                return null;

            DeleteResult result = await _orders.DeleteOneAsync(x => x.Id == id);

            if (!result.IsAcknowledged)
                return null;

            return result.DeletedCount == 1 ? order : null;
        }

        public async Task<IEnumerable<OrderMongo>> GetAllObjects()
        {
            return await _orders.FindAsync(FilterDefinition<OrderMongo>.Empty).Result.ToListAsync();
        }

        public async Task<IEnumerable<OrderMongo>> GetAllObjects(Expression<Func<OrderMongo, bool>>? filter = null)
        {
            return await _orders.FindAsync(filter).Result.ToListAsync();
        }

        public async Task<OrderMongo?> GetObject(int id)
        {
            return await _orders.FindAsync(x => x.Id == id).Result.FirstOrDefaultAsync();
        }

        public async Task<OrderMongo?> UpdateObject(int id, OrderMongo obj)
        {
            OrderMongo? oldOrder = await GetObject(id);
            if (oldOrder is null)
                return null;

            if (!obj.Validate())
                throw new Exception("Validation error");

            if (id != obj.Id)
                throw new Exception("Ids must match in URI and body");

            if (oldOrder is not null && _emailService is not null)
            {
                string projectCategory = string.Empty;
                if (oldOrder.Status.Category == 1 && obj.Status.Category == 2)
                    projectCategory = "Planlægning";
                else if (oldOrder.Status.Category == 3 && obj.Status.Category == 4)
                    projectCategory = "Feedback";
                await _emailService.SendNotificationEmail(obj.Contact.Name, obj.Contact.Email, obj.ProjectName, projectCategory);
            }

            ReplaceOneResult result = await _orders.ReplaceOneAsync(x => x.Id == obj.Id, obj);

            if (result.IsAcknowledged)
                return result.ModifiedCount == 1 ? obj : throw new Exception("Error in database");

            return null;
        }
    }
}
