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
        private readonly IMongoCollection<Order> _orders;
        private readonly IEmailService _emailService;

        public OrderService(MongoDbContext mongoDbService, IEmailService emailService)
        {
            _orders = mongoDbService.Database?.GetCollection<Order>(Secrets.MongoCollectionName)!;
            _emailService = emailService;
        }

        public async Task AddObject(Order obj)
        {
            if (!obj.Validate())
                return;

            Order? lastOrder = null;
            if (await _orders.CountDocumentsAsync(FilterDefinition<Order>.Empty) > 0)
                lastOrder = await _orders.Find(FilterDefinition<Order>.Empty).SortByDescending(o => o.Id).FirstAsync();

            obj.Id = lastOrder != null ? ++lastOrder.Id : 1000;

            await _orders.InsertOneAsync(obj);
        }

        public async Task<Order?> DeleteObject(int id)
        {
            Order? order = await GetObject(id);

            if (order is null)
                return null;

            DeleteResult result = await _orders.DeleteOneAsync(x => x.Id == id);

            if (!result.IsAcknowledged)
                return null;

            return result.DeletedCount == 1 ? order : null;
        }

        public async Task<IEnumerable<Order>> GetAllObjects()
        {
            return await _orders.FindAsync(FilterDefinition<Order>.Empty).Result.ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllObjects(Expression<Func<Order, bool>>? filter = null)
        {
            return await _orders.FindAsync(filter).Result.ToListAsync();
        }

        public async Task<Order?> GetObject(int id)
        {
            return await _orders.FindAsync(x => x.Id == id).Result.FirstOrDefaultAsync();
        }

        public async Task<Order?> UpdateObject(int id, Order obj)
        {
            Order? oldOrder = await GetObject(id);
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
