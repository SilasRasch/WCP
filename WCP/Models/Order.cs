using WCPShared.Models.UserModels;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Models
{
    public class Order
    {
        #region Base

        public int Id { get; set; }
        public double Price { get; set; }
        public int DeliveryTimeFrom { get; set; }
        public int DeliveryTimeTo { get; set; }
        public int State { get; set; }
        public int Category { get; set; }
        //public int OrganizationId { get; set; }
        //public Organization Organization { get; set; } = new Organization();
        public List<Creator> Creators { get; set; } = new List<Creator>();

        // Drive-links
        public string Scripts { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty; // Dump for creator
        public string Delivery { get; set; } = string.Empty; // Finished product
        public string Other { get; set; } = string.Empty;

        #endregion

        #region Page one

        public int BrandId { get; set; }
        public Brand Brand { get; set; } = new Brand();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        #endregion

        #region Page two

        public string ProjectName { get; set; } = string.Empty;
        public string ProjectType { get; set; } = string.Empty;
        public int ContentCount { get; set; }
        public int? ContentLength { get; set; }
        public string Platforms { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;

        #endregion

        #region Page three

        public bool? ExtraCreator { get; set; }
        public int? ExtraHook { get; set; }
        public string? ExtraNotes { get; set; }
        public string? FocusPoints { get; set; }
        public string? RelevantFiles { get; set; }
        public List<string> Ideas { get; set; } = new List<string>();
        public List<string> Products { get; set; } = new List<string>();

        #endregion

        public bool Validate()
        {
            if (Validation.ValidateEmail(Email))
                return false;

            if (Validation.ValidatePhone(Phone))
                return false;

            if (Validation.ValidateDisplayName(Name))
                return false;

            if (!Brand.Validate())
                return false;

            if (string.IsNullOrWhiteSpace(ProjectName) || BrandId == 0 || string.IsNullOrWhiteSpace(ProjectType) || string.IsNullOrWhiteSpace(Platforms) || string.IsNullOrWhiteSpace(Format))
                return false;

            return true;
        }
    }
}
