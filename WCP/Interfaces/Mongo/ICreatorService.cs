using WCPShared.Models.UserModels.CreatorModels;

namespace WCPShared.Interfaces.Mongo
{
    public interface ICreatorService : IDatabaseService<CreatorMongo>, IMongoDbServiceExtension<CreatorMongo>
    {
    }
}
