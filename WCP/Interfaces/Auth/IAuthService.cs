using WCPShared.Models.Entities.AuthModels;
using WCPShared.Models.Entities.UserModels;

namespace WCPShared.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<User?> Register(RegisterCreatorDto request, bool selfRegister = false);
        Task<User?> Register(User user, Creator? creator = null, bool selfRegister = false);
        Task<AuthResponse?> Login(UserDto request);
        Task LoginAttempt(bool success, User user);
        Task<bool> CheckLoginAttempts(UserDto request);
        Task<User?> SelfRegister(SelfRegisterDto request);
        Task<User?> Verify(VerifyUserDto request);
        Task<dynamic> Authenticate();
        Task<bool> AddAdmin(int id);
    }
}
