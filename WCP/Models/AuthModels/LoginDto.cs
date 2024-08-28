using System.ComponentModel.DataAnnotations;

namespace WCPShared.Models.AuthModels
{
    public class LoginDto
    {
        [Required, DataType(DataType.EmailAddress), EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
