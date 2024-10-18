using Microsoft.EntityFrameworkCore;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models.DTOs;
using WCPShared.Interfaces;
using WCPShared.Models.Views;
using System.Linq.Expressions;
using WCPShared.Services.Converters;
using System;
using WCPShared.Models.Entities;

namespace WCPShared.Services.EntityFramework
{
    public class OrganizationService : GenericEFService<Organization>, IDtoExtensions<OrganizationDto, Organization>, IObjectViewService<Organization, OrganizationView>
    {
        private readonly IWcpDbContext _context;
        private readonly ViewConverter _viewConverter;
        private readonly LanguageService _languageService;

        public OrganizationService(IWcpDbContext context, ViewConverter viewConverter, LanguageService languageService) : base(context)
        {
            _context = context;
            _viewConverter = viewConverter;
            _languageService = languageService;
        }

        public async Task<Organization?> GetObject(int id, bool includeBrands = false)
        {
            if (includeBrands)
                return await _context.Organizations.Include(x => x.Brands).SingleOrDefaultAsync(x => x.Id == id);
            return await _context.Organizations.Include(x => x.Language).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Organization>> GetAllObjects(bool includeBrands = false)
        {
            if (includeBrands)
                return await _context.Organizations.Include(x => x.Brands).Include(x => x.Language).ToListAsync();
            return await _context.Organizations.Include(x => x.Language).ToListAsync();
        }

        public async Task<Organization?> UpdateObject(int id, OrganizationDto organization)
        {
            Organization? oldOrg = await GetObject(id);

            if (oldOrg is null)
                return null!;

            if (oldOrg.Language.Id != organization.LanguageId)
            {
                var lang = await _languageService.GetObject(organization.LanguageId);
                
                if (lang is not null)
                {
                    oldOrg.LanguageId = lang.Id;
                    oldOrg.Language = lang;
                }
            }

            oldOrg.Name = organization.Name;
            oldOrg.CVR = organization.CVR;

            _context.Update(oldOrg);
            await _context.SaveChangesAsync();
            return oldOrg;
        }

        public async Task<Organization?> AddObject(OrganizationDto obj)
        {
            var lang = await _languageService.GetObject(obj.LanguageId);
            
            if (lang is null)
                return null!;

            Organization organization = new Organization
            {
                CVR = obj.CVR,
                Name = obj.Name,
                LanguageId = obj.LanguageId,
                Language = lang
            };

            await _context.AddAsync(organization);
            await _context.SaveChangesAsync();
            return organization;
        }

        public async Task<List<OrganizationView>> GetObjectsViewBy(Expression<Func<Organization, bool>> predicate)
        {
            return await _context.Organizations
                .Include(x => x.Language)
                .Where(predicate)
                .Select(x => _viewConverter.Convert(x))
                .ToListAsync();
        }

        public async Task<OrganizationView?> GetObjectViewBy(Expression<Func<Organization, bool>> predicate)
        {
            var organization = await _context.Organizations.Include(x => x.Language).SingleOrDefaultAsync(predicate);

            if (organization is not null)
                return _viewConverter.Convert(organization);
            return null;
        }

        public async Task<List<OrganizationView>> GetAllObjectsView()
        {
            return await _context.Organizations
                .Include(x => x.Language)
                .Select(x => _viewConverter.Convert(x))
                .ToListAsync();
        }
    }
}
