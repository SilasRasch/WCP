using System.ComponentModel.DataAnnotations;

namespace WCPAuthAPI.Models.DTOs
{
    public class PasswordOnly
    {
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
