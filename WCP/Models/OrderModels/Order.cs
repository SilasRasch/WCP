using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WCPShared.Models.BrandModels;
using WCPShared.Models.UserModels.CreatorModels;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Models.OrderModels
{
    public class Order
    {
        #region Base

        [BsonId]
        public int Id { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("deliveryTimeFrom")]
        public int DeliveryTimeFrom { get; set; }

        [BsonElement("deliveryTimeTo")]
        public int DeliveryTimeTo { get; set; }

        [BsonElement("userId")]
        public int UserId { get; set; }

        [BsonElement("organizationId")]
        public int OrganizationId { get; set; }

        [BsonElement("status")]
        public Status Status { get; set; } = new Status();

        [BsonElement("links")]
        [BsonIgnoreIfNull]
        public Links? Links { get; set; }

        [BsonElement("creators")]
        [BsonIgnoreIfNull]
        public List<int>? Creators { get; set; }

        #endregion

        #region Page one

        [BsonElement("brand")]
        public Brand Brand { get; set; } = new Brand();
        [BsonElement("contact")]
        public Contact Contact { get; set; } = new Contact();

        #endregion

        #region Page two

        [BsonElement("projectName")]
        public string ProjectName { get; set; } = string.Empty;
        [BsonElement("projectType")]
        public string ProjectType { get; set; } = string.Empty;
        [BsonElement("contentCount")]
        public int ContentCount { get; set; }
        [BsonElement("contentLength")]
        [BsonIgnoreIfNull]
        public int ContentLength { get; set; }
        [BsonElement("platforms")]
        public string Platforms { get; set; } = string.Empty;
        [BsonElement("format")]
        public string Format { get; set; } = string.Empty;

        #endregion

        #region Page three

        [BsonElement("extraCreator")]
        [BsonIgnoreIfNull]
        public bool ExtraCreator { get; set; }
        [BsonElement("extraHook")]
        [BsonIgnoreIfDefault]
        public int ExtraHook { get; set; }
        [BsonElement("extraNotes")]
        [BsonIgnoreIfNull]
        public string? ExtraNotes { get; set; }
        [BsonElement("creatorDescription")]
        [BsonIgnoreIfNull]
        public string? CreatorDescription { get; set; }
        [BsonElement("focusPoints")]
        [BsonIgnoreIfNull]
        public string? FocusPoints { get; set; }
        [BsonElement("ideas")]
        [BsonIgnoreIfNull]
        public List<string>? Ideas { get; set; }
        [BsonElement("relevantFiles")]
        [BsonIgnoreIfNull]
        public string? RelevantFiles { get; set; }
        [BsonElement("products")]
        [BsonIgnoreIfNull]
        public List<string>? Products { get; set; }

        #endregion

        public bool Validate()
        {
            if (Validation.ValidateEmail(Contact.Email))
                return false;

            if (Validation.ValidatePhone(Contact.Phone))
                return false;

            if (Validation.ValidateDisplayName(Contact.Name))
                return false;

            if (!Brand.Validate())
                return false;

            if (String.IsNullOrWhiteSpace(ProjectName) || UserId == default || OrganizationId == default || String.IsNullOrWhiteSpace(ProjectType) || String.IsNullOrWhiteSpace(Platforms) || String.IsNullOrWhiteSpace(Format))
                return false;

            return true;
        }
    }
}
