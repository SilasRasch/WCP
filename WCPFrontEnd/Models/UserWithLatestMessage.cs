using WCPShared.Models.Entities;
using WCPShared.Models.Entities.UserModels;

namespace WCPFrontEnd.Models
{
    public class UserWithLatestMessage
    {
        public User User { get; set; }
        public ChatMessage LatestMessage { get; set; }
    }
}
