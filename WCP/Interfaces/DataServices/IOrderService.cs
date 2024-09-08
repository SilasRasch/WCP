using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Interfaces.DataServices.Utility;
using WCPShared.Models;
using WCPShared.Models.DTOs;
using WCPShared.Models.Views;

namespace WCPShared.Interfaces.DataServices
{
    public interface IOrderService : IDatabaseService<Order>, IDtoExtensions<OrderDto, Order>, IObjectViewService<Order, OrderView>
    {
        Task<List<Order>> GetObjectsBy(Expression<Func<Order, bool>> predicate);
        Task<Order?> GetObjectBy(Expression<Func<Order, bool>> predicate);
    }
}
