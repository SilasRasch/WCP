using WCPShared.Models.BrandModels;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CVR { get; set; } = string.Empty;
        public List<Brand> Brands { get; set; } = new List<Brand>();

        public bool Validate()
        {
            if (CVR is not null && !Validation.ValidateCVR(CVR))
                return false;

            if (!Validation.ValidateDisplayName(Name))
                return false;

            return true;
        }
    }
}
