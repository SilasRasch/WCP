using WCPShared.Models;
using Microsoft.EntityFrameworkCore;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models.DTOs;

namespace WCPShared.Services.Databases.EntityFramework
{
    public class OrganizationService : IOrganizationService
    {
        private readonly WcpDbContext _context;

        public OrganizationService(WcpDbContext context)
        {
            _context = context;
        }

        public async Task AddObject(Organization organization)
        {
            await _context.Organizations.AddAsync(organization);
            await _context.SaveChangesAsync();
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
            return await _context.Organizations.Include(x => x.Brands).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Organization>> GetAllObjects()
        {
            return await _context.Organizations.Include(x => x.Brands).ToListAsync();
        }

        public async Task<Organization?> GetObject(int id, bool includeBrands = false)
        {
            if (includeBrands)
                return await _context.Organizations.Include(x => x.Brands).FirstOrDefaultAsync(x => x.Id == id);
            return await _context.Organizations.FirstOrDefaultAsync(x => x.Id == id);   
        }

        public async Task<IEnumerable<Organization>> GetAllObjects(bool includeBrands = false)
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
    }
}
