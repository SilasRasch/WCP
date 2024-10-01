using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.DTOs;

namespace WCPShared.Models.Entities.AuthModels
{
    public class RegisterCreatorDto
    {
        public RegisterDto User { get; set; } = new();
        public CreatorDto? Creator { get; set; }
    }
}
