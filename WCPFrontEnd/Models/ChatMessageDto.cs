using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WCPShared.Models.Entities;

namespace WCPFrontEnd.Models
{
    public class ChatMessageDto
    {
        public int FromId { get; set; }
        public int ToId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime Sent { get; set; }
        public bool Read { get; set; }

        public ChatMessageDto(ChatMessage message) 
        {
            FromId = message.FromId;
            ToId = message.ToId;
            Message = message.Message;
            Sent = message.Sent;
            Read = message.Read;
        }
    }
}
