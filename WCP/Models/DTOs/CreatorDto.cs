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
        public bool IsEditor { get; set; }
        public int? UserId { get; set; }
        public List<string>? Languages { get; set; } = new List<string>();
        public string Gender { get; set; } = string.Empty;

        public bool Validate()
        {
            if (!IsEditor && (string.IsNullOrWhiteSpace(Address) || string.IsNullOrWhiteSpace(Gender) || string.IsNullOrWhiteSpace(Address)))
                return false;

            return true;
        }
    }
}
