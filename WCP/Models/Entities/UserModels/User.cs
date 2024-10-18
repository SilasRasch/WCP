using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WCPShared.Interfaces;
using WCPShared.Services.StaticHelpers;
using Microsoft.IdentityModel.Tokens;
using WCPShared.Models.Entities;

namespace WCPShared.Models.Entities.UserModels
{
    public class User : UserAuth, IUser, IWcpEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? OrganizationId { get; set; }
        public Organization? Organization { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }

        public bool Validate()
        {
            if (!Validation.ValidateEmail(Email))
                return false;

            if (Role != "Bruger" && Role != "Creator" && Role != "Admin")
                return false;

            if (!Phone.IsNullOrEmpty() && !Validation.ValidatePhone(Phone!))
                return false;

            if (!Validation.ValidateDisplayName(Name))
                return false;

            return true;
        }
    }
}
