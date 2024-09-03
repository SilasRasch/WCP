using WCPShared.Interfaces.DataServices;
using WCPShared.Models;
using Microsoft.EntityFrameworkCore;
using WCPShared.Models.DTOs;
using EllipticCurve.Utils;
using WCPShared.Models.UserModels;

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
            return await _context.Orders.Include(x => x.Brand).ThenInclude(b => b.Organization).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Order>> GetAllObjects()
        {
            return await _context.Orders.Include(x => x.Brand).ThenInclude(b => b.Organization).ToListAsync();
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

            existingOrder.BrandId = order.BrandId;
            existingOrder.Price = order.Price;
            existingOrder.Category = 0;
            existingOrder.State = 0;
            existingOrder.Content = order.Content;
            existingOrder.ContentCount = order.ContentCount;
            existingOrder.ContentLength = order.ContentLength;
            existingOrder.Delivery = order.Delivery;
            existingOrder.DeliveryTimeFrom = order.DeliveryTimeFrom;
            existingOrder.DeliveryTimeTo = order.DeliveryTimeTo;
            existingOrder.Email = order.Email;
            existingOrder.Name = order.Name;
            existingOrder.Phone = order.Phone;
            existingOrder.ExtraCreator = order.ExtraCreator;
            existingOrder.ExtraHook = order.ExtraHook;
            existingOrder.ExtraNotes = order.ExtraNotes;
            existingOrder.FocusPoints = order.FocusPoints;
            existingOrder.Format = order.Format;
            existingOrder.Ideas = order.Ideas;
            existingOrder.Platforms = order.Platforms;
            existingOrder.Products = order.Products;
            existingOrder.ProjectName = order.ProjectName;
            existingOrder.ProjectType = order.ProjectType;
            existingOrder.RelevantFiles = order.RelevantFiles;
            existingOrder.Scripts = order.Scripts;
            existingOrder.Other = order.Other;

            if (order.BrandId != existingOrder.BrandId)
            {
                Brand? brand = await _brandService.GetObject(order.BrandId);
                if (brand is not null)
                    existingOrder.Brand = brand;
            }

            if (order.Creators is not null)
            {
                var newCreators = _creatorService.GetAllObjects().Result.Where(x => order.Creators.Contains(x.Id)).ToList();

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

            var order = new Order
            {
                Brand = brand,
                Creators = creators,
                BrandId = obj.BrandId,
                Price = obj.Price,
                Category = 0,
                State = 0,
                Content = obj.Content,
                ContentCount = obj.ContentCount,
                ContentLength = obj.ContentLength,
                Delivery = obj.Delivery,
                DeliveryTimeFrom = obj.DeliveryTimeFrom,
                DeliveryTimeTo = obj.DeliveryTimeTo,
                Email = obj.Email,
                Name = obj.Name,
                Phone = obj.Phone,
                ExtraCreator = obj.ExtraCreator,
                ExtraHook = obj.ExtraHook,
                ExtraNotes = obj.ExtraNotes,
                FocusPoints = obj.FocusPoints,
                Format = obj.Format,
                Ideas = obj.Ideas,
                Platforms = obj.Platforms,
                Products = obj.Products,
                ProjectName = obj.ProjectName,
                ProjectType = obj.ProjectType,
                RelevantFiles = obj.RelevantFiles,
                Scripts = obj.Scripts,
                Other = obj.Other
            };

            await _context.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
