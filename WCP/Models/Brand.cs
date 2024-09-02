using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Models
{
    public class Brand
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; } = new();
        public List<Order> Orders { get; set; } = new();

        public bool Validate()
        {
            if (!Validation.ValidateBrandURL(URL))
                return false;

            if (Validation.ValidateDisplayName(Name))
                return false;

            return true;
        }
    }
}
