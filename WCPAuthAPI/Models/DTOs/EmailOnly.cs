using System.ComponentModel.DataAnnotations;

namespace WCPAuthAPI.Models.DTOs
{
    public class EmailOnly
    {
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
