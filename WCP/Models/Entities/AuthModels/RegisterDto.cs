using Microsoft.IdentityModel.Tokens;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Models.Entities.AuthModels
{
    public class RegisterDto
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Role { get; set; } = string.Empty;
        public int? OrganizationId { get; set; } = null!;

        public bool Validate()
        {
            if (!Validation.ValidateEmail(Email))
                return false;

            if (Role != "Bruger" && Role != "Creator" && Role != "Admin" && Role != "Editor")
                return false;

            if (!Phone.IsNullOrEmpty() && !Validation.ValidatePhone(Phone!))
                return false;

            if (!Name.IsNullOrEmpty() && !Validation.ValidateDisplayName(Name))
                return false;

            return true;
        }
    }
}
