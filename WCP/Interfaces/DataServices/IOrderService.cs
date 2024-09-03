using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models;
using WCPShared.Models.DTOs;

namespace WCPShared.Interfaces.DataServices
{
    public interface IOrderService : IDatabaseService<Order>, IDtoExtensions<OrderDto, Order>
    {
    }
}
