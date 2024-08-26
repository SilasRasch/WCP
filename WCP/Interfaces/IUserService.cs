using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.UserModels;

namespace WCPShared.Interfaces
{
    public interface IUserService : IDatabaseService<User>
    {
        Task<User?> GetUserByVerificationToken(string token);
        Task<User?> GetUserByResetToken(string resetToken);
        Task<User?> GetUserByEmail(string email);
    }
}
