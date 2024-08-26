using System.ComponentModel.DataAnnotations;

namespace WCPAuthAPI.Models.DTOs
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
