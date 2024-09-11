using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.AuthModels;
using WCPShared.Models.UserModels;

namespace WCPShared.Models.DTOs
{
    public class RegisterCreatorDto
    {
        public RegisterDto User { get; set; } = new();
        public CreatorDto? Creator { get; set; }
    }
}
