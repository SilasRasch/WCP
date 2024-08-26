using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.OrderModels;
using WCPShared.Models.UserModels.CreatorModels;

namespace WCPShared.Interfaces
{
    public interface IOrderService : IDatabaseService<Order>
    {
        Task<IEnumerable<Order>> GetAllObjects(Expression<Func<Order, bool>>? filter = null);
    }
}
