﻿using WCPShared.Interfaces.DataServices;
using WCPShared.Models;
using Microsoft.EntityFrameworkCore;
using WCPShared.Models.DTOs;
using WCPShared.Models.Views;
using System.Linq.Expressions;
using WCPShared.Services.Converters;
using System;
using WCPShared.Interfaces;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Models.Enums;

namespace WCPShared.Services.EntityFramework
{
    public class CreatorService : GenericEFService<Creator>, IDtoExtensions<CreatorDto, Creator>, IObjectViewService<Creator, CreatorView>
    {
        private readonly IWcpDbContext _context;
        private readonly UserService _userService;
        private readonly LanguageService _languageService;
        private readonly ViewConverter _viewConverter;

        public CreatorService(IWcpDbContext context, LanguageService languageService, UserService userService, ViewConverter viewConverter) : base(context)
        {
            _context = context;
            _languageService = languageService;
            _userService = userService;
            _viewConverter = viewConverter;
        }

        public async Task<Creator?> UpdateObject(int id, CreatorDto obj)
        {
            Creator? oldCreator = await GetObject(id);

            if (oldCreator is null)
                return null!;

            oldCreator.DateOfBirth = obj.DateOfBirth;
            oldCreator.Address = obj.Address;
            oldCreator.ImgURL = obj.ImgURL;
            oldCreator.Gender = obj.Gender;
            oldCreator.SubType = (CreatorSubType)Enum.Parse(typeof(CreatorSubType), obj.SubType);

            if (obj.Languages is not null)
            {
                var newLanguages = (await _languageService.GetAllObjects()).Where(x => obj.Languages.Contains(x.Name)).ToList();

                if (oldCreator.Languages is not null)
                {
                    oldCreator.Languages.Clear();
                    oldCreator.Languages = newLanguages;
                }
            }

            if (obj.UserId is not null && obj.UserId != oldCreator.UserId)
            {
                var user = await _userService.GetObject(obj.UserId.Value);
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

        public async Task<Creator?> AddObject(CreatorDto obj)
        {
            if (obj.UserId is null || !obj.Validate()) return null!;

            var user = await _userService.GetObject(obj.UserId.Value);
            if (user is null)
                return null;

            var creatorToAdd = new Creator
            {
                Address = obj.Address,
                Gender = obj.Gender,
                DateOfBirth = obj.DateOfBirth,
                ImgURL = obj.ImgURL,
                SubType = (CreatorSubType)Enum.Parse(typeof(CreatorSubType), obj.SubType),
                Languages = new List<Language>(),
                UserId = obj.UserId.Value,
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
                return _viewConverter.Convert(creator);
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
