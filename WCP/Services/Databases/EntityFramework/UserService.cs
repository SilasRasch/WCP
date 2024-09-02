using WCPShared.Models.UserModels;
using WCPShared.Models;
using Microsoft.EntityFrameworkCore;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models.AuthModels;
using Azure.Core;

namespace WCPShared.Services.Databases.EntityFramework
{
    public class UserService : IUserService
    {
        private readonly WcpDbContext _context;

        public UserService(WcpDbContext context)
        {
            _context = context;
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
            return await _context.Users.Include(x => x.Organization).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetUserByResetToken(string resetToken)
        {
            return await _context.Users.Include(x => x.Organization).FirstOrDefaultAsync(x => x.PasswordResetToken == resetToken);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.Include(x => x.Organization).FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetUserByVerificationToken(string token)
        {
            return await _context.Users.Include(x => x.Organization).FirstOrDefaultAsync(x => x.VerificationToken == token);
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
    }
}
