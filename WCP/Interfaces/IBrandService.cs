using System.Linq.Expressions;
using WCPShared.Models.BrandModels;

namespace WCPShared.Interfaces
{
    public interface IBrandService : IDatabaseService<Brand>
    {
        Task<IEnumerable<Brand>> GetAllObjects(Expression<Func<Brand, bool>>? filter = null);
    }
}
