using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.BrandModels;
using WCPShared.Models.UserModels.CreatorModels;

namespace WCPShared.Interfaces
{
    public interface ICreatorService : IDatabaseService<Creator>
    {
        Task<IEnumerable<Creator>> GetAllObjects(Expression<Func<Creator, bool>>? filter = null);
    }
}
