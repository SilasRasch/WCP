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
        public UserNC User { get; set; } = new UserNC();
        public CreatorDto Creator { get; set; } = new CreatorDto();

        public CreatorView(Creator obj)
        {
            obj.Id = Id;
            Creator.Address = obj.Address;
            Creator.Gender = obj.Gender;
            Creator.UserId = obj.UserId;
            Creator.IsEditor = obj.IsEditor;
            Creator.Languages = obj.Languages.Select(x => x.Name).ToList();
            Creator.DateOfBirth = obj.DateOfBirth;
            Creator.Speciality = obj.Speciality;
            Creator.ImgURL = obj.ImgURL;
        }
    }
}
