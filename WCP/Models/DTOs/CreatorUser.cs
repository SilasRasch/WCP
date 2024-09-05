using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.UserModels;

namespace WCPShared.Models.DTOs
{
    public class CreatorUser
    {
        public int Id { get; set; }
        public UserNC User { get; set; } = new UserNC();
        public CreatorDto Creator { get; set; } = new CreatorDto();
    }
}
