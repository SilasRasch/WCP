using Microsoft.EntityFrameworkCore;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models;

namespace WCPShared.Services.Databases.EntityFramework
{
    public class BrandService : IBrandService
    {
        private readonly WcpDbContext _context;

        public BrandService(WcpDbContext context)
        {
            _context = context;
        }

        public async Task AddObject(Brand brand)
        {
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
            return await _context.Brands.Include(x => x.Organization).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Brand>> GetAllObjects()
        {
            return await _context.Brands.Include(x => x.Organization).ToListAsync();
        }

        public async Task<Brand?> UpdateObject(int id, Brand brand)
        {
            Brand? oldOrg = await GetObject(id);

            if (oldOrg is null || id != brand.Id)
                return null!;

            _context.Update(brand);
            await _context.SaveChangesAsync();
            return brand;
        }
    }
}
