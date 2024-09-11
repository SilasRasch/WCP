using WCPShared.Models;
using Microsoft.EntityFrameworkCore;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models.DTOs;
using WCPShared.Interfaces;
using WCPShared.Models.Views;
using System.Linq.Expressions;
using WCPShared.Services.Converters;
using System;

namespace WCPShared.Services.Databases.EntityFramework
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IWcpDbContext _context;
        private readonly ViewConverter _viewConverter;

        public OrganizationService(IWcpDbContext context, ViewConverter viewConverter)
        {
            _context = context;
            _viewConverter = viewConverter;
        }

        public async Task<Organization> AddObject(Organization organization)
        {
            await _context.Organizations.AddAsync(organization);
            await _context.SaveChangesAsync();
            return organization;
        }

        public async Task<Organization?> DeleteObject(int id)
        {
            Organization? organization = await GetObject(id);

            if (organization is null)
                return null!;

            _context.Organizations.Remove(organization);
            await _context.SaveChangesAsync();
            return organization;
        }

        public async Task<Organization?> GetObject(int id)
        {
            return await _context.Organizations.Include(x => x.Brands).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Organization>> GetAllObjects()
        {
            return await _context.Organizations.Include(x => x.Brands).ToListAsync();
        }

        public async Task<Organization?> GetObject(int id, bool includeBrands = false)
        {
            if (includeBrands)
                return await _context.Organizations.Include(x => x.Brands).SingleOrDefaultAsync(x => x.Id == id);
            return await _context.Organizations.SingleOrDefaultAsync(x => x.Id == id);   
        }

        public async Task<List<Organization>> GetAllObjects(bool includeBrands = false)
        {
            if (includeBrands)
                return await _context.Organizations.Include(x => x.Brands).ToListAsync();
            return await _context.Organizations.ToListAsync();
        }

        public async Task<Organization?> UpdateObject(int id, Organization organization)
        {
            Organization? oldOrg = await GetObject(id);

            if (oldOrg is null || id != organization.Id)
                return null!;

            _context.Update(organization);
            await _context.SaveChangesAsync();
            return organization;
        }

        public async Task<Organization?> UpdateObject(int id, OrganizationDto organization)
        {
            Organization? oldOrg = await GetObject(id);

            if (oldOrg is null)
                return null!;

            oldOrg.Name = organization.Name;
            oldOrg.CVR = organization.CVR;

            _context.Update(oldOrg);
            await _context.SaveChangesAsync();
            return oldOrg;
        }

        public async Task<Organization?> AddObject(OrganizationDto obj)
        {
            Organization organization = new Organization
            {
                CVR = obj.CVR,
                Name = obj.Name
            };

            await _context.AddAsync(organization);
            await _context.SaveChangesAsync();
            return organization;
        }

        public async Task<List<OrganizationView>> GetObjectsViewBy(Expression<Func<Organization, bool>> predicate)
        {
            return await _context.Organizations
                .Where(predicate)
                .Select(x => _viewConverter.Convert(x))
                .ToListAsync();
        }

        public async Task<OrganizationView?> GetObjectViewBy(Expression<Func<Organization, bool>> predicate)
        {
            var organization = await _context.Organizations.SingleOrDefaultAsync(predicate);

            if (organization is not null)
                return _viewConverter.Convert(organization);
            return null;
        }

        public async Task<List<OrganizationView>> GetAllObjectsView()
        {
            return await _context.Organizations
                .Select(x => _viewConverter.Convert(x))
                .ToListAsync();
        }
    }
}
