using WCPShared.Models.UserModels;
using WCPShared.Models;
using Microsoft.EntityFrameworkCore;
using WCPShared.Interfaces.DataServices;

namespace WCPShared.Services.Databases.EntityFramework
{
    public class UserService : IUserService
    {
        private readonly AuthDbContext _context;

        public UserService(AuthDbContext context)
        {
            _context = context;
        }

        public async Task AddObject(User user)
        {
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
            return await _context.Users.Include(x => x.Organization).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetUserByResetToken(string resetToken)
        {
            return await _context.Users.Include(x => x.Organization).AsNoTracking().FirstOrDefaultAsync(x => x.PasswordResetToken == resetToken);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.Include(x => x.Organization).AsNoTracking().FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetUserByVerificationToken(string token)
        {
            return await _context.Users.Include(x => x.Organization).AsNoTracking().FirstOrDefaultAsync(x => x.VerificationToken == token);
        }

        public async Task<List<User>> GetAllObjects()
        {
            return await _context.Users.Include(x => x.Organization).AsNoTracking().ToListAsync();
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
    }
}
