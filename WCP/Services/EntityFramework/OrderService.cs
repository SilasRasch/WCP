using WCPShared.Interfaces.DataServices;
using WCPShared.Models;
using Microsoft.EntityFrameworkCore;
using WCPShared.Models.DTOs;
using WCPShared.Models.UserModels;
using WCPShared.Services.StaticHelpers;
using System.Linq.Expressions;
using WCPShared.Models.Views;
using WCPShared.Services.Converters;
using WCPShared.Interfaces;

namespace WCPShared.Services.EntityFramework
{
    public class OrderService : IOrderService
    {
        private readonly IWcpDbContext _context;
        private readonly IBrandService _brandService;
        private readonly ICreatorService _creatorService;
        private readonly ViewConverter _viewConverter;
        private readonly SlackNotificationService _slackNetService;

        public OrderService(IWcpDbContext context, IBrandService brandService, ICreatorService creatorService, ViewConverter viewConverter, SlackNotificationService slackNetService)
        {
            _context = context;
            _brandService = brandService;
            _creatorService = creatorService;
            _viewConverter = viewConverter;
            _slackNetService = slackNetService;
        }

        public async Task<Order> AddObject(Order obj)
        {
            if (obj.Brand is not null)
                _context.Brands.Attach(obj.Brand);
            if (obj.Creators is not null)
                _context.Creators.AttachRange(obj.Creators);

            await _context.Orders.AddAsync(obj);
            await _context.SaveChangesAsync();
            return obj;
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
            return await _context.Orders
                .Include(x => x.Brand)
                .ThenInclude(b => b.Organization)
                .Include(x => x.Creators)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Order?> GetObjectBy(Expression<Func<Order, bool>> predicate)
        {
            return await _context.Orders
                .Include(x => x.Brand)
                .ThenInclude(b => b.Organization)
                .Include(x => x.Creators)
                .SingleOrDefaultAsync(predicate);
        }

        public async Task<List<Order>> GetAllObjects()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<List<Order>> GetObjectsBy(Expression<Func<Order, bool>> predicate)
        {
            return await _context.Orders
                .Where(predicate)
                .Include(x => x.Brand)
                .ThenInclude(b => b.Organization)
                .Include(x => x.Creators)
                .ToListAsync();
        }

        public async Task<Order?> UpdateObject(int id, Order obj)
        {
            Order? existingOrder = await GetObject(id);

            if (existingOrder is null)
                return null!;

            _context.ChangeTracker.Clear();
            _context.Update(obj);
            await _context.SaveChangesAsync();
            await _slackNetService.SendStatusNotifications(obj, existingOrder);

            return obj;
        }

        public async Task<Order?> UpdateObject(int id, OrderDto order)
        {
            Order? existingOrder = await GetObject(id);
            if (existingOrder is null)
                return null;

            Order copyOfExistingOrder = DtoConverter.CloneOrder(existingOrder);

            existingOrder.BrandId = order.BrandId;
            existingOrder.Price = order.Price;
            existingOrder.Status = order.Status;
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
            await _slackNetService.SendStatusNotifications(existingOrder, copyOfExistingOrder);

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
            order.Status = 0;

            await _context.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<OrderView>> GetAllObjectsView()
        {
            return await _context.Orders
                .Include(x => x.Brand)
                .ThenInclude(b => b.Organization)
                .Include(x => x.Creators)
                .ThenInclude(x => x.User)
                .Select(x => _viewConverter.Convert(x))
                .ToListAsync();
        }

        public async Task<List<OrderView>> GetObjectsViewBy(Expression<Func<Order, bool>> predicate)
        {
            return await _context.Orders
                .Where(predicate)
                .Include(x => x.Brand)
                .ThenInclude(b => b.Organization)
                .Include(x => x.Creators)
                .ThenInclude(x => x.User)
                .Select(x => _viewConverter.Convert(x))
                .ToListAsync();
        }

        public async Task<OrderView?> GetObjectViewBy(Expression<Func<Order, bool>> predicate)
        {
            var order = await _context.Orders
                .Include(x => x.Brand)
                .ThenInclude(b => b.Organization)
                .Include(x => x.Creators)
                .SingleOrDefaultAsync(predicate);

            if (order is not null)
                return _viewConverter.Convert(order);
            return null;
        }
    }
}
