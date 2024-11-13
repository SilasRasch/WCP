using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using WCPShared.Interfaces;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Models.Entities
{
    public class Organization : IWcpEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CVR { get; set; } = string.Empty;
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public Subscription Subscription { get; set; }
        public List<Brand> Brands { get; set; } = new List<Brand>();
        public string StripeAccountId { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        
        public bool Validate()
        {
            if (CVR is not null && !Validation.ValidateCVR(CVR))
                return false;

            if (!Validation.ValidateDisplayName(Name))
                return false;

            if (Language is null)
                return false;

            return true;
        }
    }
}
