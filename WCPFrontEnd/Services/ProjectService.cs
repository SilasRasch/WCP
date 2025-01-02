using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WCPShared.Interfaces;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.ProjectModels;
using WCPShared.Models.Enums;
using WCPShared.Services;
using WCPShared.Services.EntityFramework;

namespace WCPFrontEnd.Services
{
    public class ProjectService : GenericEFService<Project>
    {
        private readonly IWcpDbContext _context;
        private readonly StripeService _stripeService;
        private readonly SlackNotificationService _slackNotificationService;
        private readonly ShippingService _shippingService;

        public ProjectService(IWcpDbContext context, StripeService stripeService, SlackNotificationService slackNotificationService, ShippingService shippingService)
            : base(context)
        {
            _context = context;
            _stripeService = stripeService;
            _slackNotificationService = slackNotificationService;
            _shippingService = shippingService;
        }

        public override async Task<Project?> AddObject(Project obj)
        {
            obj.Status = ProjectStatus.Unconfirmed;
            obj.Created = DateTime.Now;
            obj.Updated = DateTime.Now;

            await _context.AddAsync(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        public async Task<Project?> UpdateObject(Project obj, Project oldObj)
        {
            obj.Updated = DateTime.Now;

            _context.Update(obj);
            await _context.SaveChangesAsync();

            if (obj is CreatorProject creatorProject && oldObj is CreatorProject oldCreatorProject)
            {
                PayCreators(creatorProject, oldCreatorProject);
                await CreateShippingLabels(creatorProject, oldCreatorProject);
            }

            await _slackNotificationService.SendStatusNotifications(obj, oldObj);;
            return obj;
        }

        public override async Task<Project?> UpdateObject(int id, Project project)
        {
            project.Updated = DateTime.Now;

            _context.Update(project);
            await _context.SaveChangesAsync();
            return project;
        }

        private void PayCreators(CreatorProject project, CreatorProject oldProject)
        {
            if (project.Status == ProjectStatus.Finished && oldProject.Status == ProjectStatus.Feedback)
            {
                foreach (CreatorParticipation participation in project.Participations)
                {
                    if (string.IsNullOrEmpty(participation.Creator.StripeAccountId))
                    {
                        var res = _stripeService.Transfer(participation.Salary, participation.Creator.StripeAccountId, $"Projektløn ({project.Id})", participation.Creator.User.Language.Currency);
                        
                        if (res is not null)
                        {
                            participation.HasBeenPaid = true;
                            _context.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        private async Task CreateShippingLabels(CreatorProject project, CreatorProject oldProject)
        {
            if (project.Brand.Organization.Name.ToLower() == "webshopskolen")
            {
                if (project.Status == ProjectStatus.CreatorFilming && oldProject.Status == ProjectStatus.Planned)
                {
                    foreach (CreatorParticipation participation in project.Participations)
                    {
                        if (participation.Creator.SubType == CreatorSubType.UGC)
                        {
                            var res = await _shippingService.CreateShipment($"{participation.Project.Id}");
                            participation.ShipmentId = res.Id;
                            await _context.SaveChangesAsync();
                            await _shippingService.SendShippingEmail(participation, res.Labels.First().Base64);
                        }
                    }
                }
            }
        }

        public async Task<CreatorProject?> CreatorDelivery(CreatorProject project, int creatorId)
        {
            CreatorParticipation? participation = project.Participations
                .SingleOrDefault(p => p.CreatorId == creatorId);

            if (participation is not null)
                participation.HasDelivered = true;

            // If all scripters have delivered, send to planning
            var scripters = project.Participations.Where(x => x.Creator.SubType == CreatorSubType.Scripter);
            if (scripters.All(x => x.HasDelivered) && project.Status == ProjectStatus.Scripting)
                project.Status = ProjectStatus.Planned;

            // If all UGC creators have delivered, send to editing
            var ugcCreators = project.Participations.Where(x => x.Creator.SubType == CreatorSubType.UGC);
            bool notEmpty = ugcCreators.Any();
            if (ugcCreators.All(x => x.HasDelivered) && ugcCreators.Any() && project.Status == ProjectStatus.CreatorFilming)
                project.Status = ProjectStatus.Editing;

            _context.Update(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<List<Project>> GetAllProjects() 
        {
            return await _context.Projects
                .Include(x => x.Brand)
                .ThenInclude(b => b.Organization)
                .ThenInclude(x => x.Language)
                .Include(x => (x as CreatorProject).Participations)
                .ThenInclude(x => x.Creator)
                .ThenInclude(x => x.User)
                .ThenInclude(x => x.Language)
                .Include(x => x.Concepts)
                .ThenInclude(x => x.Product)
                .AsSplitQuery()
                .OrderBy(x => x.Status)
                .ToListAsync();
        }

        public async Task<List<Project>> GetProjectsBy(Expression<Func<Project, bool>> predicate)
        {
            return await _context.Projects
                .Include(x => x.Brand)
                .ThenInclude(b => b.Organization)
                .ThenInclude(x => x.Language)
                .Include(x => (x as CreatorProject).Participations)
                .ThenInclude(x => x.Creator)
                .ThenInclude(x => x.User)
                .ThenInclude(x => x.Language)
                .Include(x => x.Concepts)
                .ThenInclude(x => x.Product)
                .Where(predicate)
                .AsSplitQuery()
                .OrderBy(x => x.Status)
                .ToListAsync();
        }

        public async Task<Project?> GetProjectBy(Expression<Func<Project, bool>> predicate)
        {
            return await _context.Projects
                .Include(x => x.Brand)
                .ThenInclude(b => b.Organization)
                .ThenInclude(x => x.Language)
                .Include(x => (x as CreatorProject).Participations)
                .ThenInclude(x => x.Creator)
                .ThenInclude(x => x.User)
                .ThenInclude(x => x.Language)
                .Include(x => x.Concepts)
                .ThenInclude(x => x.Product)
                .AsSplitQuery()
                .SingleOrDefaultAsync(predicate);
        }
    }
}
