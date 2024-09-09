using WCPShared.Interfaces.DataServices;
using WCPShared.Models;
using Microsoft.EntityFrameworkCore;
using WCPShared.Models.UserModels;
using WCPShared.Models.DTOs;
using WCPShared.Models.Views;
using System.Linq.Expressions;
using WCPShared.Services.Converters;
using System;
using WCPShared.Interfaces;

namespace WCPShared.Services.Databases.EntityFramework
{
    public class CreatorService : ICreatorService
    {
        private readonly IWcpDbContext _context;
        private readonly IUserService _userService;
        private readonly ILanguageService _languageService;
        private readonly ViewConverter _viewConverter;

        public CreatorService(IWcpDbContext context, ILanguageService languageService, IUserService userService, ViewConverter viewConverter)
        {
            _context = context;
            _languageService = languageService;
            _userService = userService;
            _viewConverter = viewConverter;
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
            return await _context.Creators.Include(x => x.Languages).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Creator>> GetAllObjects()
        {
            return await _context.Creators.Include(x => x.Languages).ToListAsync();
        }

        public async Task<Creator?> UpdateObject(int id, CreatorDto obj)
        {
            Creator? oldCreator = await GetObject(id);

            if (oldCreator is null)
                return null!;

            oldCreator.DateOfBirth = obj.DateOfBirth;
            oldCreator.Address = obj.Address;
            oldCreator.Speciality = obj.Speciality;
            oldCreator.ImgURL = obj.ImgURL;
            oldCreator.Gender = obj.Gender;

            if (obj.Languages is not null)
            {
                var newLanguages = (await _languageService.GetAllObjects()).Where(x => obj.Languages.Contains(x.Name)).ToList();

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

        public async Task<Creator?> AddObject(CreatorDto obj)
        {
            var user = await _userService.GetObject(obj.UserId);
            if (user is null)
                return null;
            
            var creatorToAdd = new Creator
            {
                Address = obj.Address,
                Gender = obj.Gender,
                DateOfBirth = obj.DateOfBirth,
                ImgURL = obj.ImgURL,
                IsEditor = obj.IsEditor,
                Languages = new List<Language>(),
                Speciality = obj.Speciality,
                UserId = obj.UserId,
                User = user
            };

            if (obj.Languages is not null)
            {
                var languages = await _languageService.GetAllObjects();
                creatorToAdd.Languages = languages.Where(x => obj.Languages.Contains(x.Name)).ToList();
            }

            await _context.Creators.AddAsync(creatorToAdd);
            await _context.SaveChangesAsync();
            return creatorToAdd;
        }

        public async Task<List<CreatorView>> GetObjectsViewBy(Expression<Func<Creator, bool>> predicate)
        {
            return await _context.Creators
                .Where(predicate)
                .Include(x => x.Languages)
                .Include(x => x.User)
                .Select(x => _viewConverter.Convert(x))
                .ToListAsync();
        }

        public async Task<CreatorView?> GetObjectViewBy(Expression<Func<Creator, bool>> predicate)
        {
            var creator = await _context.Creators
                .Include(x => x.Languages)
                .Include(x => x.User)
                .SingleOrDefaultAsync(predicate);

            if (creator is not null)
                _viewConverter.Convert(creator);
            return null;
        }

        public async Task<List<CreatorView>> GetAllObjectsView()
        {
            return await _context.Creators
                .Include(x => x.Languages)
                .Include(x => x.User)
                .Select(x => _viewConverter.Convert(x))
                .ToListAsync();
        }
    }
}
