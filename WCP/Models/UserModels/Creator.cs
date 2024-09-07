using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WCPShared.Models.UserModels
{
    public class Creator : IEquatable<Creator?>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; } = string.Empty;
        public string? Speciality { get; set; } = string.Empty;
        public string? ImgURL { get; set; }
        public bool IsEditor { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = new User();
        public List<Language>? Languages { get; set; } = new List<Language>();
        public List<Order> Orders { get; set; } = new List<Order>();
        public string? Gender {  get; set; } = string.Empty;

        public bool Validate()
        {
            if (!IsEditor && string.IsNullOrWhiteSpace(Address))
                return false;

            return true;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Creator);
        }

        public bool Equals(Creator? other)
        {
            return other is not null &&
                   Id == other.Id;
        }
    }
}
