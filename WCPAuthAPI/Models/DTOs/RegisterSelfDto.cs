using WCPShared.Models.DTOs;

namespace WCPAuthAPI.Models.DTOs
{
    public class RegisterSelfDto : RegisterCreatorDto
    {
        public string Password { get; set; } = string.Empty;
        public string VerificationToken { get; set; } = string.Empty;
    }
}
