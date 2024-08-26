using WCPAuthAPI.Models;
using WCPAuthAPI.Models.DTOs;
using WCPShared.Models.UserModels;

namespace WCPAuthAPI.Services.JWTs
{
    public interface ITokenService
    {
        Task<AuthResponse?> Login(UserDTO request);
        Task<User> Register(RegisterDto request);
        Task<bool> AddAdmin(int id);
        string CreateToken(User user);
        Task<string?> RefreshToken(int userId, string refreshToken);
        Task<string?> RefreshToken(string email, string refreshToken);
        Task RevokeSession(int userId);
        string GenerateRandomString(int byteCount);
        Task<bool> CheckLoginAttempts(UserDTO request);
    }
}
