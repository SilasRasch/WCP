using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Interfaces.DataServices.Utility;
using WCPShared.Models.UserModels;
using WCPShared.Models.Views;

namespace WCPShared.Interfaces.DataServices
{
    public interface IUserService : IDatabaseService<User>, IDtoExtensions<RegisterDto, User>, IObjectViewService<User, UserView>
    {
        Task<User?> GetUserByVerificationToken(string token);
        Task<User?> GetUserByResetToken(string resetToken);
        Task<User?> GetUserByEmail(string email);
    }
}
