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
            if (obj.Videographer is not null)
                _context.Creators.Attach(obj.Videographer);
            if (obj.Editor is not null)
                _context.Creators.Attach(obj.Editor);

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
                .Include(x => x.Videographer)
                .Include(x => x.Editor)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Order?> GetObjectBy(Expression<Func<Order, bool>> predicate)
        {
            return await _context.Orders
                .Include(x => x.Brand)
                .ThenInclude(b => b.Organization)
                .Include(x => x.Creators)
                .Include(x => x.Videographer)
                .Include(x => x.Editor)
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
                .Include(x => x.Videographer)
                .Include(x => x.Editor)
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
            //existingOrder.CreatorDeliveryStatus = order.CreatorDeliveryStatus;

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

            if (order.VideographerId is not null)
            {
                Creator? newVideographer = await _creatorService.GetObject(order.VideographerId.Value);
                if (newVideographer is not null)
                    existingOrder.Videographer = newVideographer;
            }

            if (order.EditorId is not null)
            {
                Creator? newEditor = await _creatorService.GetObject(order.EditorId.Value);
                if (newEditor is not null)
                    existingOrder.Videographer = newEditor;
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
            //order.CreatorDeliveryStatus = creators.ToDictionary(x => x.Id, x => false);
            order.Status = 0;

            if (obj.VideographerId is not null)
            {
                order.Videographer = await _creatorService.GetObject(obj.VideographerId.Value);

                //if (order.Videographer is not null)
                //    order.CreatorDeliveryStatus.Add(obj.VideographerId.Value, false);
            }
                
            if (obj.EditorId is not null)
            {
                order.Editor = await _creatorService.GetObject(obj.EditorId.Value);

                //if (order.Editor is not null)
                //    order.CreatorDeliveryStatus.Add(obj.EditorId.Value, false);
            }
                

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
                .Include(x => x.Videographer)
                .ThenInclude(x => x.User)
                .Include(x => x.Editor)
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
                .Include(x => x.Videographer)
                .ThenInclude(x => x.User)
                .Include(x => x.Editor)
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
                .ThenInclude(x => x.User)
                .Include(x => x.Videographer)
                .ThenInclude(x => x.User)
                .Include(x => x.Editor)
                .ThenInclude(x => x.User)
                .SingleOrDefaultAsync(predicate);

            if (order is not null)
                return _viewConverter.Convert(order);
            return null;
        }
    }
}
