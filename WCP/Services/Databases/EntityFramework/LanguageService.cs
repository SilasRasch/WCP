using Microsoft.EntityFrameworkCore;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models;
using WCPShared.Models.UserModels;

namespace WCPShared.Services.Databases.EntityFramework
{
    public class LanguageService : ILanguageService
    {
        private readonly WcpDbContext _context;

        public LanguageService(WcpDbContext context)
        {
            _context = context;
        }

        public async Task AddObject(Language language)
        {            
            await _context.Languages.AddAsync(language);
            await _context.SaveChangesAsync();
        }

        public async Task<Language?> DeleteObject(int id)
        {
            Language? language = await GetObject(id);

            if (language is null)
                return null!;

            _context.Languages.Remove(language);
            await _context.SaveChangesAsync();
            return language;
        }

        public async Task<Language?> GetObject(int id)
        {
            return await _context.Languages.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Language>> GetAllObjects()
        {
            return await _context.Languages.ToListAsync();
        }

        public async Task<Language?> UpdateObject(int id, Language language)
        {
            Language? oldLang = await GetObject(id);

            if (oldLang is null || id != language.Id)
                return null!;

            _context.ChangeTracker.Clear();
            _context.Update(language);
            await _context.SaveChangesAsync();
            return language;
        }
    }
}
