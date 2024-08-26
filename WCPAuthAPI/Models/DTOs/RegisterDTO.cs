using Microsoft.IdentityModel.Tokens;
using WCPShared.Models;
using WCPShared.Services.StaticHelpers;

namespace WCPAuthAPI.Models.DTOs
{
    public class RegisterDTO
    {
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? CVR { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string Role { get; set; } = string.Empty;
        public Organization? Organization { get; set; } = null!;

        public bool Validate()
        {
            if (!Validation.ValidateEmail(Email))
                return false;

            if (Role != "Bruger" && Role != "Creator" && Role != "Admin" && Role != "Editor")
                return false;

            if (!Phone.IsNullOrEmpty() && !Validation.ValidatePhone(Phone!))
                return false;

            if (!Validation.ValidateDisplayName(DisplayName))
                return false;

            return true;
        }
    }
}
