using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WCPShared.Services.StaticHelpers;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Interfaces;
using WCPShared.Models.Enums;

namespace WCPShared.Models.Entities
{
    public class Order : IEquatable<Order?>, IWcpEntity
    {
        #region Base

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double Price { get; set; }

        // Legacy (should be removed when moving to v2.0)
        public int DeliveryTimeFrom { get; set; }
        public int DeliveryTimeTo { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int Status { get; set; }
        //public ProjectStatus StatusEnum { get; set; }

        // All creators should be in participations
        public int? VideographerId { get; set; }
        public Creator? Videographer { get; set; }
        public int? EditorId { get; set; }
        public Creator? Editor { get; set; }

        public List<StaticTemplate>? StaticTemplates { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public List<CreatorParticipation> Participations { get; set; } = [];

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
        //public ProjectType ProjectTypeEnum { get; set; }
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
        public List<Idea> Ideas { get; set; } = [];
        public List<Product> Products { get; set; } = [];

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

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
