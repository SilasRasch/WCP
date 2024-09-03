using Microsoft.EntityFrameworkCore;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models;

namespace WCPShared.Services.Databases.EntityFramework
{
    public class BrandService : IBrandService
    {
        private readonly WcpDbContext _context;
        private readonly IOrganizationService _organizationService;

        public BrandService(WcpDbContext context, IOrganizationService organizationService)
        {
            _context = context;
            _organizationService = organizationService;
        }

        public async Task AddObject(Brand brand)
        {
            _context.Organizations.Attach(brand.Organization);
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
        }

        public async Task<Brand?> DeleteObject(int id)
        {
            Brand? brand = await GetObject(id);

            if (brand is null)
                return null!;

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        public async Task<Brand?> GetObject(int id)
        {
            return await _context.Brands.Include(x => x.Organization).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Brand>> GetAllObjects()
        {
            return await _context.Brands.Include(x => x.Organization).ToListAsync();
        }

        public async Task<Brand?> UpdateObject(int id, Brand brand)
        {
            Brand? oldBrand = await GetObject(id);

            if (oldBrand is null || id != brand.Id)
                return null!;

            //_context.ChangeTracker.Clear();
            _context.Update(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        public async Task<Brand?> UpdateObject(int id, BrandDto brand)
        {
            Brand? oldBrand = await GetObject(id);
            
            if (oldBrand is null)
                return null!;

            oldBrand.Name = brand.Name;
            oldBrand.URL = brand.URL;

            if (oldBrand.OrganizationId != brand.OrganizationId)
            {
                var org = await _organizationService.GetObject(brand.OrganizationId);

                if (org is not null)
                {
                    oldBrand.OrganizationId = brand.OrganizationId;
                    oldBrand.Organization = org;
                }
            }

            _context.Update(oldBrand);
            await _context.SaveChangesAsync();
            return oldBrand;
        }

        public async Task<Brand?> AddObject(BrandDto obj)
        {
            Organization? organization = await _organizationService.GetObject(obj.OrganizationId);
            if (organization is null)
                return null;

            _context.Organizations.Attach(organization);
            Brand brand = new Brand
            {
                Name = obj.Name,
                OrganizationId = obj.OrganizationId,
                Organization = organization,
                URL = obj.URL,
            };

            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
            return brand;
        }
    }
}
