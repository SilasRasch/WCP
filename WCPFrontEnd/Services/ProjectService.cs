using Microsoft.EntityFrameworkCore;
using Stripe.Climate;
using System.Linq.Expressions;
using WCPShared.Interfaces;
using WCPShared.Models.DTOs;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.ProjectModels;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Models.Enums;
using WCPShared.Services;
using WCPShared.Services.EntityFramework;
using WCPShared.Services.StaticHelpers;

namespace WCPFrontEnd.Services
{
    public class ProjectService : GenericEFService<Project>
    {
        private readonly IWcpDbContext _context;
        private readonly StripeService _stripeService;
        private readonly SlackNotificationService _slackNotificationService;

        public ProjectService(IWcpDbContext context, StripeService stripeService, SlackNotificationService slackNotificationService)
            : base(context)
        {
            _context = context;
            _stripeService = stripeService;
            _slackNotificationService = slackNotificationService;
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
            }

            await _slackNotificationService.SendStatusNotifications(obj, oldObj);
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
                    if (participation.Creator.StripeAccountId is not null)
                    {
                        _stripeService.Transfer(participation.Salary, participation.Creator.StripeAccountId, $"Projektløn ({project.Id})", participation.Creator.User.Language.Currency);
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
                .Include(x => x.Product)
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
                .Include(x => x.Product)
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
                .Include(x => x.Product)
                .AsSplitQuery()
                .SingleOrDefaultAsync(predicate);
        }
    }
}
