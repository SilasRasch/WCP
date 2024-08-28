using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Models.UserModels
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string? CVR { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? PasswordResetToken { get; set; } = string.Empty;
        public string? VerificationToken { get; set; } = string.Empty;
        public DateTime? ResetTokenExpiry { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public int LoginTries { get; set; }
        public DateTime? LastLoginAttempt { get; set; }
        public int? OrganizationId { get; set; }
        public Organization? Organization { get; set; }

        public bool Validate()
        {
            if (!Validation.ValidateEmail(Email))
                return false;

            if (Role != "Bruger" || Role != "Creator" || Role != "Admin" || Role != "Editor")
                return false;

            if (Phone is not null && !Validation.ValidatePhone(Phone))
                return false;

            if (!Validation.ValidateDisplayName(DisplayName))
                return false;

            return true;
        }

        public UserNC ToUserNC()
        {
            return new UserNC
            {
                CVR = CVR,
                DisplayName = DisplayName,
                Email = Email,
                Id = Id,
                IsActive = IsActive,
                Organization = Organization,
                Phone = Phone,
                Role = Role
            };
        }
    }
}
