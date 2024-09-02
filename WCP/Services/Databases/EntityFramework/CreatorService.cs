using WCPShared.Interfaces.DataServices;
using WCPShared.Models;
using Microsoft.EntityFrameworkCore;
using WCPShared.Models.UserModels;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Metadata;
using WCPShared.Models.DTOs;

namespace WCPShared.Services.Databases.EntityFramework
{
    public class CreatorService : ICreatorService
    {
        private readonly WcpDbContext _context;
        private readonly IUserService _userService;
        private readonly ILanguageService _languageService;

        public CreatorService(WcpDbContext context, ILanguageService languageService, IUserService userService)
        {
            _context = context;
            _languageService = languageService;
            _userService = userService;
        }

        public async Task AddObject(Creator obj)
        {
            if (obj.Languages is not null)
                _context.Languages.AttachRange(obj.Languages);
            if (obj.User is not null)
                _context.Users.Attach(obj.User);

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
            return await _context.Creators.Include(x => x.User).Include(x => x.Languages).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Creator?> GetObject(int id, bool includeUser = false)
        {
            if (includeUser)
                return await _context.Creators.Include(x => x.User).Include(x => x.Languages).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return await GetObject(id);
        }

        public async Task<List<Creator>> GetAllObjects()
        {
            return await _context.Creators.Include(x => x.Languages).AsNoTracking().ToListAsync();
        }

        public async Task<List<Creator>> GetAllObjects(bool includeUser = false)
        {
            if (includeUser)
                return await _context.Creators.Include(x => x.User).Include(x => x.Languages).AsNoTracking().ToListAsync();
            else return await GetAllObjects();
        }

        public async Task<Creator?> UpdateObject(int id, CreatorDto obj)
        {
            Creator? oldCreator = await GetObject(id);

            if (oldCreator is null)
                return null!;

            _context.Creators.Attach(oldCreator);

            oldCreator.DateOfBirth = obj.DateOfBirth;
            oldCreator.Address = obj.Address;
            oldCreator.Speciality = obj.Speciality;
            oldCreator.ImgURL = obj.ImgURL;

            if (obj.Languages is not null)
            {
                var newLanguages = _languageService.GetAllObjects().Result.Where(x => obj.Languages.Contains(x.Name)).ToList();

                if (oldCreator.Languages is not null)
                {
                    oldCreator.Languages.Clear();
                    oldCreator.Languages = newLanguages;
                }
            }

            if (obj.UserId != oldCreator.UserId)
            {
                var user = await _userService.GetObject(obj.UserId);
                if (user is not null)
                {
                    oldCreator.UserId = user.Id;
                    oldCreator.User = user;
                }
            }

            _context.Update(oldCreator);
            await _context.SaveChangesAsync();
            return oldCreator;
        }

        public async Task<Creator?> UpdateObject(int id, Creator obj)
        {
            Creator? oldCreator = await GetObject(id);

            if (oldCreator is null)
                return null!;

            _context.Update(oldCreator);
            await _context.SaveChangesAsync();
            return oldCreator;
        }

        public void Attach(Creator creator)
        {
            _context.Creators.Attach(creator);
        }
    }
}
