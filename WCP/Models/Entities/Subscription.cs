using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WCPShared.Models.Enums;

namespace WCPShared.Models.Entities
{
    public class Subscription
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string StripeSubscriptionId { get; set; } = string.Empty;
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public SubscriptionType Type { get; set; }
        public int NumberOfVideos { get; set; }
        public int NumberOfBrands { get; set; }
        public int NumberOfUsers { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastPaid { get; set; }
    }
}
