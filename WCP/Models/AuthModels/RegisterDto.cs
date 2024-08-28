using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Models.AuthModels
{
    public class RegisterDto
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string DisplayName { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? CVR { get; set; } = string.Empty;
        
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
