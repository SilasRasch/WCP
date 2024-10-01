﻿using Microsoft.EntityFrameworkCore;
using WCPShared.Interfaces;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models;
using WCPShared.Models.Entities.UserModels;

namespace WCPShared.Services.EntityFramework
{
    public class LanguageService : IDatabaseService<Language>
    {
        private readonly IWcpDbContext _context;

        public LanguageService(IWcpDbContext context)
        {
            _context = context;
        }

        public async Task<Language> AddObject(Language language)
        {
            await _context.Languages.AddAsync(language);
            await _context.SaveChangesAsync();
            return language;
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
            return await _context.Languages.SingleOrDefaultAsync(x => x.Id == id);
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
