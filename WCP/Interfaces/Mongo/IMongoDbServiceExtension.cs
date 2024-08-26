using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.BrandModels;

namespace WCPShared.Interfaces.Mongo
{
    public interface IMongoDbServiceExtension<T> where T : class
    {
        Task<IEnumerable<T>> GetAllObjects(Expression<Func<T, bool>>? filter = null);
    }
}
