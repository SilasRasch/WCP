using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WCPShared.Services.StaticHelpers;
using WCPShared.Interfaces;
using WCPShared.Models.Entities.ProjectModels;

namespace WCPShared.Models.Entities
{
    public class Brand : IWcpEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; } = new();
        public List<Product> Products { get; set; } = [];
        public List<Project> Projects { get; set; } = [];

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
