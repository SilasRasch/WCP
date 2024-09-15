using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WCPShared.Models.UserModels;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Models
{
    public class Order : IEquatable<Order?>
    {
        #region Base

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double Price { get; set; }
        public int DeliveryTimeFrom { get; set; }
        public int DeliveryTimeTo { get; set; }
        public int Status { get; set; }
        public int? VideographerId { get; set; }
        public Creator? Videographer { get; set; }
        public int? EditorId { get; set; }
        public Creator? Editor { get; set; }
        public List<Creator> Creators { get; set; } = [];
        public List<StaticTemplate> StaticTemplates { get; set; } = [];
        //public Dictionary<int, bool> CreatorDeliveryStatus { get; set; } = new Dictionary<int, bool>();

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
            if (!Validation.ValidateEmail(Email))
                return false;

            if (!Validation.ValidatePhone(Phone))
                return false;

            if (!Validation.ValidateDisplayName(Name))
                return false;

            if (!Brand.Validate())
                return false;

            if (string.IsNullOrWhiteSpace(ProjectName) || BrandId == 0 || string.IsNullOrWhiteSpace(ProjectType) || string.IsNullOrWhiteSpace(Platforms) || string.IsNullOrWhiteSpace(Format))
                return false;

            return true;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Order);
        }

        public bool Equals(Order? other)
        {
            return other is not null &&
                   Id == other.Id;
        }
    }
}
