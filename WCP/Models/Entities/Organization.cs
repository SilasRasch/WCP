using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Models.Entities
{
    public class Organization
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
