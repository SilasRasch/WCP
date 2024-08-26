using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Models.BrandModels
{
    public class Brand
    {
        [BsonId]
        public int Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("url")]
        public string URL { get; set; } = string.Empty;
        [BsonElement("organizationId")]
        public int OrganizationId { get; set; }
        [BsonElement("userId")]
        public int userId { get; set; }

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
