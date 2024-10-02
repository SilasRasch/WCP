using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WCPShared.Interfaces;
using WCPShared.Interfaces.DataServices;

namespace WCPShared.Services.EntityFramework
{
    public class GenericEFService<T> : IDatabaseService<T>, IDatabaseServiceExtensions<T>
        where T : class, IWcpEntity
    {
        private readonly IWcpDbContext _context;

        public GenericEFService(IWcpDbContext context)
        {
            _context = context;    
        }

        public virtual async Task<T?> AddObject(T obj)
        {
            await _context.Set<T>()
                .AddAsync(obj);
            
            await _context.SaveChangesAsync();
            return obj;
        }

        public virtual async Task<List<T>> GetAllObjects()
        {
            return await _context.Set<T>()
                .ToListAsync();
        }

        public virtual async Task<List<T>> GetObjectsBy(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>()
                .Where(predicate)
                .ToListAsync();
        }

        public virtual async Task<T?> GetObjectBy(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>()
                .FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<T?> GetObject(int id)
        {
            return await _context.Set<T>()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<T?> UpdateObject(int id, T obj)
        {
            if (!await ExistsAsync(id)) return null;
            
            _context.Update(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        public virtual async Task<T?> DeleteObject(int id)
        {
            if (!await ExistsAsync(id)) return null;
            T? obj = await _context.Set<T>().SingleOrDefaultAsync(x => x.Id == id);

            _context.Set<T>().Remove(obj!);
            await _context.SaveChangesAsync();
            return obj;
        }

        private async Task<bool> ExistsAsync(int id) => await _context.Set<T>().AnyAsync(x => x.Id == id);
    }
}
