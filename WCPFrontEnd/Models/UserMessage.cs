using WCPShared.Models.Entities.UserModels;

namespace WCPFrontEnd.Models
{
    public class UserMessage
    {
        public User UserFrom { get; set; }
        public User UserTo { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime DateSent { get; set; }
    }
}
