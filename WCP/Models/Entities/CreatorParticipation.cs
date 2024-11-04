
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WCPShared.Interfaces;
using WCPShared.Models.Entities.UserModels;

namespace WCPShared.Models.Entities
{
    public class CreatorParticipation
    {
        public int CreatorId { get; set; }
        public Creator Creator { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public float Salary { get; set; }
        public bool HasDelivered { get; set; }
    }
}
