using System.ComponentModel.DataAnnotations;

namespace WCPAuthAPI.Models.DTOs
{
    public class ResetPasswordDto
    {
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
