using WCPShared.Models.OrderModels;

namespace WCPShared.Interfaces.Mongo
{
    public interface IOrderService : IDatabaseService<OrderMongo>, IMongoDbServiceExtension<OrderMongo>
    {
    }
}
