using WCPShared.Models.Entities.ProjectModels;
using WCPShared.Models.Entities.UserModels;

namespace WCPShared.Models.Entities
{
    public class Feedback
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public User User { get; set; }
        public int ProjectId { get; set; }
        public int Score { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime Created { get; set; }
    }
}