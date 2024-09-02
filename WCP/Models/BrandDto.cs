using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Models
{
    public class BrandDto
    {
        public string Name { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public int OrganizationId { get; set; }

        public bool Validate()
        {
            if (!Validation.ValidateBrandURL(URL))
                return false;

            if (!Validation.ValidateDisplayName(Name))
                return false;

            return true;
        }
    }
}
