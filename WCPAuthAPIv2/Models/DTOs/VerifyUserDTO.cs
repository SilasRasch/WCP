using System.ComponentModel.DataAnnotations;

namespace WCPAuthAPI.Models.DTOs
{
    public class VerifyUserDTO
    {
        [Required]
        public string VerificationToken { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
