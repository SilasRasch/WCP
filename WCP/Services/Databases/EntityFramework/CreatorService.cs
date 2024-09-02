using WCPShared.Interfaces.DataServices;
using WCPShared.Models;
using Microsoft.EntityFrameworkCore;
using WCPShared.Models.UserModels;

namespace WCPShared.Services.Databases.EntityFramework
{
    public class CreatorService : ICreatorService
    {
        private readonly WcpDbContext _context;

        public CreatorService(WcpDbContext context)
        {
            _context = context;
        }

        public async Task AddObject(Creator obj)
        {
            await _context.Creators.AddAsync(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Creator?> DeleteObject(int id)
        {
            Creator? obj = await GetObject(id);

            if (obj is null)
                return null!;

            _context.Creators.Remove(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        public async Task<Creator?> GetObject(int id)
        {
            return await _context.Creators.Include(x => x.User).Include(x => x.Languages).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Creator>> GetAllObjects()
        {
            return await _context.Creators.Include(x => x.User).Include(x => x.Languages).ToListAsync();
        }

        public async Task<Creator?> UpdateObject(int id, Creator obj)
        {
            Creator? oldOrg = await GetObject(id);

            if (oldOrg is null || id != obj.Id)
                return null!;

            _context.Update(obj);
            await _context.SaveChangesAsync();
            return obj;
        }
    }
}
