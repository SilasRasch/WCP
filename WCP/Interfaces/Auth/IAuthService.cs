using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.AuthModels;
using WCPShared.Models.UserModels;

namespace WCPShared.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<User> Register(RegisterDto request);
        Task<AuthResponse?> Login(UserDto request);
        Task LoginAttempt(bool success, User user);
        Task<bool> CheckLoginAttempts(UserDto request);
    }
}
