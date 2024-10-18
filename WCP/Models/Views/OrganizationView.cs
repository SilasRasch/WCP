using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.UserModels;

namespace WCPShared.Models.Views
{
    public class OrganizationView
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CVR { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;

        public OrganizationView(Organization obj)
        {
            Id = obj.Id;
            Name = obj.Name;
            CVR = obj.CVR;
            Language = obj.Language.Name;
        }
    }
}
