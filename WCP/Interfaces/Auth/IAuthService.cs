using WCPShared.Models.AuthModels;
using WCPShared.Models.DTOs;
using WCPShared.Models.UserModels;

namespace WCPShared.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<User> Register(RegisterDto request);
        Task<User> Register(RegisterCreatorDto request);
        Task<AuthResponse?> Login(UserDto request);
        Task LoginAttempt(bool success, User user);
        Task<bool> CheckLoginAttempts(UserDto request);
    }
}
