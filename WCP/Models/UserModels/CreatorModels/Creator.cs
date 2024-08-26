using MongoDB.Bson.Serialization.Attributes;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Models.UserModels.CreatorModels
{
    public class Creator
    {
        [BsonId]
        public int Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;
        [BsonElement("sex")]
        [BsonIgnoreIfNull]
        public string? Sex { get; set; } = string.Empty;
        [BsonElement("age")]
        [BsonIgnoreIfNull]
        public string? Age { get; set; } = string.Empty;
        [BsonElement("dateOfBirth")]
        [BsonIgnoreIfNull]
        public DateTime? DateOfBirth { get; set; }
        [BsonElement("languages")]
        [BsonIgnoreIfNull]
        public List<string>? Languages { get; set; } = new List<string>();
        [BsonElement("handles")]
        public CreatorHandles Handles { get; set; } = new CreatorHandles();
        [BsonElement("location")]
        public string? Location { get; set; } = string.Empty;
        [BsonElement("speciality")]
        public string? Speciality { get; set; } = string.Empty;
        [BsonElement("imgURL")]
        [BsonIgnoreIfNull]
        public string? ImgURL { get; set; }
        [BsonElement("isEditor")]
        public bool IsEditor { get; set; }

        public bool Validate()
        {
            if (!Validation.ValidateEmail(Email))
                return false;

            if (!Validation.ValidateDisplayName(Name))
                return false;

            if (!IsEditor && (String.IsNullOrWhiteSpace(Location) || String.IsNullOrWhiteSpace(Speciality)))
                return false;

            return true;
        }
    }
}
