using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models;
using WCPShared.Models.DTOs;
using WCPShared.Models.Views;

namespace WCPShared.Interfaces.DataServices
{
    public interface IOrderService : IDatabaseService<Order>, IDtoExtensions<OrderDto, Order>
    {
        Task<List<Order>> GetObjectsBy(Expression<Func<Order, bool>> predicate);
        Task<Order?> GetObjectBy(Expression<Func<Order, bool>> predicate);
        Task<List<OrderView>> GetObjectsViewBy(Expression<Func<Order, bool>> predicate);
        Task<OrderView?> GetObjectViewBy(Expression<Func<Order, bool>> predicate);
        Task<List<OrderView>> GetAllObjectsView();
    }
}
