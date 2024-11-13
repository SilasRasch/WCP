
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WCPShared.Interfaces;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models.DTOs;
using WCPShared.Models.Entities;
using WCPShared.Models.Views;
using WCPShared.Services.Converters;

namespace WCPShared.Services.EntityFramework
{
    public class StaticTemplateService : GenericEFService<StaticTemplate>, IObjectViewService<StaticTemplate, StaticTemplateView>, IDtoExtensions<StaticTemplateDto, StaticTemplate>
    {
        private readonly IWcpDbContext _context;
        private readonly ViewConverter _viewConverter;

        public StaticTemplateService(IWcpDbContext context, ViewConverter viewConverter) : base(context)
        {
            _context = context;
            _viewConverter = viewConverter;
        }

        public async Task<List<StaticTemplateView>> GetAllObjectsView()
        {
            return await _context.StaticTemplates
                .Include(x => x.Projects)
                .Select(x => _viewConverter.Convert(x))
                .ToListAsync();
        }

        public async Task<List<StaticTemplateView>> GetObjectsViewBy(Expression<Func<StaticTemplate, bool>> predicate)
        {
            return await _context.StaticTemplates
                .Include(x => x.Projects)
                .Where(predicate)
                .Select(x => _viewConverter.Convert(x))
                .ToListAsync();
        }

        public async Task<StaticTemplateView?> GetObjectViewBy(Expression<Func<StaticTemplate, bool>> predicate)
        {
            var template = await _context.StaticTemplates
                .Include(x => x.Projects)
                .SingleOrDefaultAsync(predicate);

            if (template is not null)
                return _viewConverter.Convert(template);
            return null;
        }

        public async Task<StaticTemplate?> AddObject(StaticTemplateDto obj)
        {
            StaticTemplate template = new StaticTemplate
            {
                Name = obj.Name,
                DisplayName = obj.DisplayName,
                TemplateImgOne = obj.TemplateImgOne,
                TemplateImgTwo = obj.TemplateImgTwo,
                ExampleImg = obj.ExampleImg,
            };

            await _context.StaticTemplates.AddAsync(template);
            await _context.SaveChangesAsync();
            return template;
        }

        public async Task<StaticTemplate?> UpdateObject(int id, StaticTemplateDto obj)
        {
            StaticTemplate? existingObject = await GetObject(id);

            if (existingObject is null)
                return null;

            existingObject.Name = obj.Name;
            existingObject.DisplayName = obj.DisplayName;
            existingObject.TemplateImgOne = obj.TemplateImgOne;
            existingObject.TemplateImgTwo = obj.TemplateImgTwo;
            existingObject.ExampleImg = obj.ExampleImg;

            _context.Update(existingObject);
            await _context.SaveChangesAsync();
            return existingObject;
        }
    }
}
