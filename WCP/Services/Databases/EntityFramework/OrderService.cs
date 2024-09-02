using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models;
using Microsoft.EntityFrameworkCore;

namespace WCPShared.Services.Databases.EntityFramework
{
    public class OrderService : IOrderService
    {
        private readonly WcpDbContext _context;

        public OrderService(WcpDbContext context)
        {
            _context = context;
        }

        public async Task AddObject(Order obj)
        {
            if (obj.Brand is not null)
                _context.Brands.Attach(obj.Brand);
            if (obj.Creators is not null)
                _context.Creators.AttachRange(obj.Creators);

            await _context.Orders.AddAsync(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Order?> DeleteObject(int id)
        {
            Order? obj = await GetObject(id);

            if (obj is null)
                return null!;

            _context.Orders.Remove(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        public async Task<Order?> GetObject(int id)
        {
            return await _context.Orders.Include(x => x.Brand).ThenInclude(b => b.Organization).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Order>> GetAllObjects()
        {
            return await _context.Orders.Include(x => x.Brand).ThenInclude(b => b.Organization).AsNoTracking().ToListAsync();
        }

        public async Task<Order?> UpdateObject(int id, Order obj)
        {
            Order? oldOrg = await GetObject(id);

            if (oldOrg is null || id != obj.Id) 
                return null!;

            _context.ChangeTracker.Clear();
            _context.Update(obj);
            await _context.SaveChangesAsync();
            return obj;
        }
    }
}
