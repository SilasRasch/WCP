using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.UserModels;

namespace WCPShared.Models.DTOs
{
    public class CreatorDto
    {
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; } = string.Empty;
        public string? Speciality { get; set; } = string.Empty;
        public string? ImgURL { get; set; }
        public string SubType { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public List<string>? Languages { get; set; } = new List<string>();
        public string? Gender { get; set; } = string.Empty;

        public bool Validate()
        {
            if (SubType.IsNullOrEmpty())
                return false;

            if (SubType == "UGC" && (string.IsNullOrWhiteSpace(Address) || string.IsNullOrWhiteSpace(Gender)))
                return false;

            return true;
        }
    }
}
