using WCPShared.Models.Entities.ProjectModels;
using WCPShared.Models.Entities.UserModels;

namespace WCPShared.Models.Entities
{
    public class CreatorParticipation
    {
        public int CreatorId { get; set; }
        public required Creator Creator { get; set; }
        public int ProjectId { get; set; }
        public required CreatorProject Project { get; set; }
        public float Salary { get; set; } = 0;
        public string Currency { get; set; } = string.Empty;
        public bool HasDelivered { get; set; } = false;
    }
}
