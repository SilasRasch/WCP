using WCPShared.Models.UserModels.CreatorModels;

namespace WCPShared.Interfaces.Mongo
{
    public interface ICreatorService : IDatabaseService<Creator>, IMongoDbServiceExtension<Creator>
    {
    }
}
