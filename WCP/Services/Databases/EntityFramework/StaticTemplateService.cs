
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WCPShared.Interfaces;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models;
using WCPShared.Models.DTOs;
using WCPShared.Models.Views;
using WCPShared.Services.Converters;

namespace WCPShared.Services.Databases.EntityFramework
{
    public class StaticTemplateService : IStaticTemplateService
    {
        private readonly IWcpDbContext _context;
        private readonly ViewConverter _viewConverter;

        public StaticTemplateService(IWcpDbContext context, ViewConverter viewConverter)
        {
            _context = context;
            _viewConverter = viewConverter;
        }

        public async Task AddObject(StaticTemplate obj)
        {
            await _context.AddAsync(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<StaticTemplate?> DeleteObject(int id)
        {
            StaticTemplate? obj = await GetObject(id);

            if (obj is null)
                return null;

            _context.StaticTemplates.Remove(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        public async Task<List<StaticTemplate>> GetAllObjects()
        {
            return await _context.StaticTemplates.ToListAsync();
        }

        public async Task<StaticTemplate?> GetObject(int id)
        {
            return await _context.StaticTemplates.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<StaticTemplate?> UpdateObject(int id, StaticTemplate obj)
        {
            StaticTemplate? existingObject = await GetObject(id);

            if (existingObject is null)
                return null;

            _context.Update(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        public async Task<List<StaticTemplateView>> GetAllObjectsView()
        {
            return await _context.StaticTemplates
                .Include(x => x.Orders)
                .Select(x => _viewConverter.Convert(x))
                .ToListAsync();
        }

        public async Task<List<StaticTemplateView>> GetObjectsViewBy(Expression<Func<StaticTemplate, bool>> predicate)
        {
            return await _context.StaticTemplates
                .Include(x => x.Orders)
                .Where(predicate)
                .Select(x => _viewConverter.Convert(x))
                .ToListAsync();
        }

        public async Task<StaticTemplateView?> GetObjectViewBy(Expression<Func<StaticTemplate, bool>> predicate)
        {
            var template = await _context.StaticTemplates
                .Include(x => x.Orders)
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
            existingObject.TemplateImgOne = obj.TemplateImgOne;
            existingObject.TemplateImgTwo = obj.TemplateImgTwo;
            existingObject.ExampleImg = obj.ExampleImg;

            _context.Update(existingObject);
            await _context.SaveChangesAsync();
            return existingObject;
        }
    }
}
