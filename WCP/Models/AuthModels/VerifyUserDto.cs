using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCPShared.Models.AuthModels
{
    public class VerifyUserDto
    {
        [Required]
        public string VerificationToken { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
