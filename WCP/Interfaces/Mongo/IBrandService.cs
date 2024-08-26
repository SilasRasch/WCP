﻿using WCPShared.Models.BrandModels;

namespace WCPShared.Interfaces.Mongo
{
    public interface IBrandService : IDatabaseService<Brand>, IMongoDbServiceExtension<Brand>
    {
    }
}
