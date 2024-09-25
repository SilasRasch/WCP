using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.AuthModels;
using WCPShared.Models.UserModels;

namespace WCPShared.Interfaces.Auth
{
    public interface IJwtService
    {
        string CreateToken(User user);
        Task<string?> RefreshToken(int userId, string refreshToken);
        Task<string?> RefreshToken(string email, string refreshToken);
        Task RevokeSession(int userId);
        string GenerateRandomString(int byteCount);
        Task<bool> ValidateToken(string token);
        Task SetRefreshToken(int userId, string token);
    }
}
