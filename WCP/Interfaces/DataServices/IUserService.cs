using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.UserModels;

namespace WCPShared.Interfaces.DataServices
{
    public interface IUserService : IDatabaseService<User>, IDtoExtensions<RegisterDto, User>
    {
        Task<User?> GetUserByVerificationToken(string token);
        Task<User?> GetUserByResetToken(string resetToken);
        Task<User?> GetUserByEmail(string email);
    }
}
