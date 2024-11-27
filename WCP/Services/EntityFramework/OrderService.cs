using WCPShared.Interfaces.DataServices;
using Microsoft.EntityFrameworkCore;
using WCPShared.Models.DTOs;
using WCPShared.Services.StaticHelpers;
using System.Linq.Expressions;
using WCPShared.Models.Views;
using WCPShared.Services.Converters;
using WCPShared.Interfaces;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Models.Enums;
using SendGrid.Helpers.Mail;

namespace WCPShared.Services.EntityFramework
{
    public class OrderService : GenericEFService<Order>, IDtoExtensions<OrderDto, Order>, IObjectViewService<Order, OrderView>
    {
        private readonly IWcpDbContext _context;
        private readonly BrandService _brandService;
        private readonly CreatorService _creatorService;
        private readonly ViewConverter _viewConverter;
        private readonly StaticTemplateService _templateService;
        private readonly SlackNotificationService _slackNetService;
        private readonly ShippingService _shippingService;

        public OrderService(IWcpDbContext context, BrandService brandService, CreatorService creatorService, ViewConverter viewConverter, SlackNotificationService slackNetService, StaticTemplateService staticTemplateService, ShippingService shippingService)
            : base(context)
        {
            _context = context;
            _brandService = brandService;
            _creatorService = creatorService;
            _viewConverter = viewConverter;
            _slackNetService = slackNetService;
            _templateService = staticTemplateService;
            _shippingService = shippingService;
        }

        public override async Task<Order?> UpdateObject(int id, Order obj)
        {
            Order? existingOrder = await GetObject(id);

            if (existingOrder is null)
                return null!;

            obj.Updated = DateTime.Now;
            
            _context.Update(obj);
            await _context.SaveChangesAsync();
            await _slackNetService.SendStatusNotifications(obj, existingOrder);

            return obj;
        }

        public async Task<Order?> UpdateObject(Order obj, Order oldObj)
        {
            obj.Updated = DateTime.Now;

            _context.Update(obj);
            await _context.SaveChangesAsync();

            await _slackNetService.SendStatusNotifications(obj, oldObj);
            await CreateShippingLabels(obj, oldObj);
            return obj;
        }
        
        public async Task<Order?> UpdateObject(int id, OrderDto order)
        {
            // Check if order exists
            Order? existingOrder = await _context.Orders
                .Include(o => o.Participations)
                .ThenInclude(x => x.Creator)
                .ThenInclude(x => x.User)
                .AsSplitQuery()
                .SingleOrDefaultAsync(o => o.Id == id);

            if (existingOrder is null)
                return null;

            // Copy order for comparison when generating notifications
            Order copyOfExistingOrder = DtoHelper.CloneOrder(existingOrder);
            
            existingOrder = DtoHelper.MapProperties(order, existingOrder);

            // Update brand
            if (order.BrandId != existingOrder.BrandId)
            {
                Brand? brand = await _brandService.GetObject(order.BrandId);
                if (brand is not null) existingOrder.Brand = brand;
            }

            // Update videographer
            if (order.VideographerId != existingOrder.VideographerId)
            {
                if (order.VideographerId is null)
                {
                    existingOrder.Videographer = null; // Removal
                }
                else
                {
                    Creator? newVideographer = await _creatorService.GetObject(order.VideographerId.Value);
                    if (newVideographer is not null)
                    {
                        existingOrder.Videographer = newVideographer;
                    }
                }
            }

            // Update editor
            if (order.EditorId != existingOrder.EditorId)
            {
                if (order.EditorId is null)
                {
                    existingOrder.Editor = null; // Removal
                }
                else
                {
                    Creator? newEditor = await _creatorService.GetObject(order.EditorId.Value);
                    if (newEditor is not null)
                    {
                        existingOrder.Editor = newEditor;
                    }
                }
            }

            existingOrder.Updated = DateTime.Now;

            // Save updates
            _context.Update(existingOrder);
            await _context.SaveChangesAsync();
            await _slackNetService.SendStatusNotifications(existingOrder, copyOfExistingOrder);

            return existingOrder;
        }

        public async Task<Order?> CreatorDelivery(Order order, int creatorId)
        {
            CreatorParticipation? participation = order.Participations
                .SingleOrDefault(p => p.CreatorId == creatorId);

            if (participation is not null)
                participation.HasDelivered = true;

            // If all scripters have delivered, send to planning
            var scripters = order.Participations.Where(x => x.Creator.SubType == CreatorSubType.Scripter);
            if (scripters.All(x => x.HasDelivered) && order.Status == ProjectStatus.Scripting)
                order.Status = ProjectStatus.Planned;

            // If all UGC creators have delivered, send to editing
            var ugcCreators = order.Participations.Where(x => x.Creator.SubType == CreatorSubType.UGC);
            bool notEmpty = ugcCreators.Any();
            if (ugcCreators.All(x => x.HasDelivered) && ugcCreators.Any() && order.Status == ProjectStatus.CreatorFilming)
                order.Status = ProjectStatus.Editing;

            _context.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> AddObject(OrderDto obj)
        {
            Brand? brand = await _brandService.GetObject(obj.BrandId);
            if (brand is null)
                return null;

            List<Creator> creators = (await _creatorService.GetAllObjects())
                .Where(x => obj.Creators.Contains(x.Id))
                .ToList();

            List<StaticTemplate> staticTemplates = (await _templateService.GetAllObjects())
                .Where(x => obj.StaticTemplates.Contains(x.Id))
                .ToList();

            Order order = DtoHelper.OrderDtoToOrder(obj);
            order.Brand = brand;
            order.StaticTemplates = staticTemplates;
            order.Status = 0;
            order.Created = DateTime.Now;

            foreach (Creator creator in creators)
            {
                if (creator is not null)
                    order.Participations.Add(new CreatorParticipation
                    {
                        Order = order,
                        Creator = creator,
                        CreatorId = creator.Id,
                        HasDelivered = false,
                        Salary = 0,
                    });
            }
                
            if (obj.VideographerId is not null)
            {
                order.Videographer = await _creatorService.GetObject(obj.VideographerId.Value);
            }
                
            if (obj.EditorId is not null)
            {
                order.Editor = await _creatorService.GetObject(obj.EditorId.Value);
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
                .ThenInclude(x => x.Language)
                .Include(x => x.Participations)
                .ThenInclude(x => x.Creator)
                .ThenInclude(x => x.User)
                .ThenInclude(x => x.Language)
                .Include(x => x.Participations)
                .ThenInclude(x => x.Creator)
                .ThenInclude(x => x.Languages)
                .Include(x => x.StaticTemplates)
                .Select(x => _viewConverter.Convert(x))
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<List<OrderView>> GetObjectsViewBy(Expression<Func<Order, bool>> predicate)
        {
            return await _context.Orders
                .Where(predicate)
                .Include(x => x.Brand)
                .ThenInclude(b => b.Organization)
                .ThenInclude(x => x.Language)
                .Include(x => x.Participations)
                .ThenInclude(x => x.Creator)
                .ThenInclude(x => x.User)
                .ThenInclude(x => x.Language)
                .Include(x => x.Participations)
                .ThenInclude(x => x.Creator)
                .ThenInclude(x => x.Languages)
                .Include(x => x.StaticTemplates)
                .Select(x => _viewConverter.Convert(x))
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<OrderView?> GetObjectViewBy(Expression<Func<Order, bool>> predicate)
        {
            var order = await _context.Orders
                .Include(x => x.Brand)
                .ThenInclude(b => b.Organization)
                .ThenInclude(x => x.Language)
                .Include(x => x.Participations)
                .ThenInclude(x => x.Creator)
                .ThenInclude(x => x.User)
                .ThenInclude(x => x.Language)
                .Include(x => x.Participations)
                .ThenInclude(x => x.Creator)
                .ThenInclude(x => x.Languages)
                .Include(x => x.StaticTemplates)
                .AsSplitQuery()
                .SingleOrDefaultAsync(predicate);

            if (order is not null)
                return _viewConverter.Convert(order);
            return null;
        }

        private async Task CreateShippingLabels(Order project, Order oldProject)
        {
            if (project.BrandId == 8 || project.BrandId == 26) // Kun Webshopskolen
            {
                if (project.Status == ProjectStatus.CreatorFilming && oldProject.Status == ProjectStatus.Planned)
                {
                    foreach (CreatorParticipation participation in project.Participations)
                    {
                        var res = await _shippingService.CreateShipment($"{participation.OrderId}");
                        //participation.ShipmentId = res.Id;
                        //await _context.SaveChangesAsync();
                        await _shippingService.SendShippingEmail(participation, res.Labels.First().Base64);
                    }
                }
            }
        }
    }
}