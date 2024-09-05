using WCPShared.Interfaces.DataServices;
using WCPShared.Models;
using Microsoft.EntityFrameworkCore;
using WCPShared.Models.DTOs;
using WCPShared.Models.UserModels;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Services.Databases.EntityFramework
{
    public class OrderService : IOrderService
    {
        private readonly WcpDbContext _context;
        private readonly IBrandService _brandService;
        private readonly ICreatorService _creatorService;

        public OrderService(WcpDbContext context, IBrandService brandService, ICreatorService creatorService)
        {
            _context = context;
            _brandService = brandService;
            _creatorService = creatorService;
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
            return await _context.Orders.Include(x => x.Brand).ThenInclude(b => b.Organization).Include(x => x.Creators).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Order>> GetAllObjects()
        {
            return await _context.Orders.Include(x => x.Brand).ThenInclude(b => b.Organization).Include(x => x.Creators).ToListAsync();
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

        public async Task<Order?> UpdateObject(int id, OrderDto order)
        {
            Order? existingOrder = await GetObject(id);

            if (existingOrder is null)
                return null;

            existingOrder = DtoConverter.OrderDtoToOrder(order);

            if (order.BrandId != existingOrder.BrandId)
            {
                Brand? brand = await _brandService.GetObject(order.BrandId);
                if (brand is not null)
                    existingOrder.Brand = brand;
            }

            if (order.Creators is not null)
            {
                var newCreators = (await _creatorService.GetAllObjects()).Where(x => order.Creators.Contains(x.Id)).ToList();

                if (existingOrder.Creators is not null)
                {
                    existingOrder.Creators.Clear();
                    existingOrder.Creators = newCreators;
                }
                else
                {
                    existingOrder.Creators = newCreators;
                }
            }

            _context.Update(existingOrder);
            await _context.SaveChangesAsync();
            return existingOrder;
        }

        public async Task<Order?> AddObject(OrderDto obj)
        {
            Brand? brand = await _brandService.GetObject(obj.BrandId);
            if (brand is null)
                return null;

            List<Creator> creators = await _creatorService.GetAllObjects();
            creators = creators.Where(x => obj.Creators.Contains(x.Id)).ToList();

            var order = DtoConverter.OrderDtoToOrder(obj);
            order.Brand = brand;
            order.Creators = creators;

            await _context.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
