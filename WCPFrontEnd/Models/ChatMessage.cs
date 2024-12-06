using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WCPShared.Interfaces;
using WCPShared.Models.Entities.UserModels;

namespace WCPFrontEnd.Models
{
    public class ChatMessage : IWcpEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public User From { get; set; }
        public int FromId { get; set; }
        public User To { get; set; }
        public int ToId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime Sent { get; set; }
        public bool Read { get; set; }

        public ChatMessage(User from, User to, string message)
        {
            From = from;
            FromId = from.Id;
            To = to;
            ToId = to.Id;
            Message = message;
            Sent = DateTime.Now;
            Read = false;
        }

        public ChatMessage()
        {
            
        }
    }
}
