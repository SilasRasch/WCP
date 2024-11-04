using Microsoft.EntityFrameworkCore;
using WCPShared.Interfaces;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models;
using WCPShared.Models.Entities.UserModels;

namespace WCPShared.Services.EntityFramework
{
    public class LanguageService : GenericEFService<Language>
    {
        private readonly IWcpDbContext _context;

        public LanguageService(IWcpDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
