using Microsoft.EntityFrameworkCore;
using WCPShared.Interfaces.DataServices;
using WCPShared.Interfaces;
using System.Linq.Expressions;
using WCPShared.Services.Converters;
using WCPShared.Models.Views;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.AuthModels;
using WCPShared.Models.Entities.UserModels;

namespace WCPShared.Services.EntityFramework
{
    public class UserService : GenericEFService<User>, IDtoExtensions<RegisterDto, User>, IObjectViewService<User, UserView>
    {
        private readonly IWcpDbContext _context;
        private readonly OrganizationService _organizationService;
        private readonly ViewConverter _viewConverter;
        private readonly LanguageService _languageService;

        public UserService(IWcpDbContext context, OrganizationService organizationService, LanguageService languageService, ViewConverter viewConverter) : base(context)
        {
            _context = context;
            _organizationService = organizationService;
            _viewConverter = viewConverter;
            _languageService = languageService;
        }

        public override async Task<User?> AddObject(User user)
        {
            if (user.Organization is not null)
                _context.Organizations.Attach(user.Organization);

            await _context.Users.AddAsync(user);
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

            Language? language = await _languageService.GetObject(obj.LanguageId);

            if (language is not null)
                _context.Languages.Attach(language);

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

        public async Task<List<UserView>> GetObjectsViewBy(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users
                .Where(predicate)
                .Include(x => x.Organization)
                .ThenInclude(x => x.Language)
                .Select(x => _viewConverter.Convert(x))
                .ToListAsync();
        }

        public async Task<UserView?> GetObjectViewBy(Expression<Func<User, bool>> predicate)
        {
            var user = await _context.Users
                .Include(x => x.Organization)
                .ThenInclude(x => x.Language)
                .SingleOrDefaultAsync(predicate);

            if (user is not null)
                return _viewConverter.Convert(user);
            return null;
        }

        public async Task<List<UserView>> GetAllObjectsView()
        {
            return await _context.Users
                .Include(x => x.Organization)
                .ThenInclude(x => x.Language)
                .Select(x => _viewConverter.Convert(x))
                .ToListAsync();
        }
    }
}
