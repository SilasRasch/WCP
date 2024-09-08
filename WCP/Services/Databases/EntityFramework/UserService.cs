using WCPShared.Models.UserModels;
using WCPShared.Models;
using Microsoft.EntityFrameworkCore;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models.AuthModels;
using Azure.Core;
using WCPShared.Interfaces;
using System.Linq.Expressions;
using WCPShared.Services.Converters;
using System;

namespace WCPShared.Services.Databases.EntityFramework
{
    public class UserService : IUserService
    {
        private readonly WcpDbContext _context;
        private readonly IOrganizationService _organizationService;
        private readonly ViewConverter _viewConverter;

        public UserService(WcpDbContext context, IOrganizationService organizationService, ViewConverter viewConverter)
        {
            _context = context;
            _organizationService = organizationService;
            _viewConverter = viewConverter;
        }

        public async Task AddObject(User user)
        {
            if (user.Organization is not null)
                _context.Organizations.Attach(user.Organization);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> DeleteObject(int id)
        {
            User? user = await GetObject(id);

            if (user is null)
                return null!;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetObject(int id)
        {
            return await _context.Users.Include(x => x.Organization).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetUserByResetToken(string resetToken)
        {
            return await _context.Users.Include(x => x.Organization).SingleOrDefaultAsync(x => x.PasswordResetToken == resetToken);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.Include(x => x.Organization).SingleOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetUserByVerificationToken(string token)
        {
            return await _context.Users.Include(x => x.Organization).SingleOrDefaultAsync(x => x.VerificationToken == token);
        }

        public async Task<List<User>> GetAllObjects()
        {
            return await _context.Users.Include(x => x.Organization).ToListAsync();
        }

        public async Task<User?> UpdateObject(int id, User user)
        {
            User? oldUser = await GetObject(id);

            if (oldUser is null || id != user.Id)
                return null!;

            _context.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateObject(int id, RegisterDto user)
        {
            User? oldUser = await GetObject(id);

            if (oldUser is null)
                return null!;

            oldUser.Email = user.Email;
            oldUser.Name = user.Name;
            oldUser.Phone = user.Phone;

            _context.Update(oldUser);
            await _context.SaveChangesAsync();
            return oldUser;
        }

        public async Task<User?> AddObject(RegisterDto obj)
        {
            Organization? organization = null;
            if (obj.OrganizationId is not null)
            {
                organization = await _organizationService.GetObject(obj.OrganizationId.Value);
            }

            if (organization is not null)
                _context.Organizations.Attach(organization);

            var user = new User
            {
                Email = obj.Email,
                Name = obj.Name,
                Phone = obj.Phone,
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<UserNC>> GetObjectsViewBy(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users
                .Where(predicate)
                .Include(x => x.Organization)
                .Select(x => _viewConverter.Convert(x))
                .ToListAsync();
        }

        public async Task<UserNC?> GetObjectViewBy(Expression<Func<User, bool>> predicate)
        {
            var user = await _context.Users
                .Include(x => x.Organization)
                .SingleOrDefaultAsync(predicate);

            if (user is not null)
                return _viewConverter.Convert(user);
            return null;
        }

        public async Task<List<UserNC>> GetAllObjectsView()
        {
            return await _context.Users
                .Include(x => x.Organization)
                .Select(x => _viewConverter.Convert(x))
                .ToListAsync();
        }
    }
}
