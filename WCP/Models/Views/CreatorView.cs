using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.DTOs;
using WCPShared.Models.UserModels;

namespace WCPShared.Models.Views
{
    public class CreatorView
    {
        public int Id { get; set; }
        public UserView User { get; set; } = new();
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; } = string.Empty;
        public string? Speciality { get; set; } = string.Empty;
        public string? ImgURL { get; set; }
        public bool IsEditor { get; set; }
        public int? UserId { get; set; }
        public List<string>? Languages { get; set; } = new List<string>();
        public string Gender { get; set; } = string.Empty;

        public CreatorView(Creator obj)
        {
            Id = obj.Id;
            Address = obj.Address;
            Gender = obj.Gender;
            UserId = obj.UserId;
            IsEditor = obj.IsEditor;
            Languages = obj.Languages.Select(x => x.Name).ToList();
            DateOfBirth = obj.DateOfBirth;
            Speciality = obj.Speciality;
            ImgURL = obj.ImgURL;
        }
    }
}
