using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using WCPFrontEnd.Models;
using WCPShared.Interfaces;
using WCPShared.Services.EntityFramework;

namespace WCPFrontEnd.Services
{
    public class ChatService : GenericEFService<ChatMessage>
    {
        public ChatService(IWcpDbContext context) : base(context)
        {
        }

        public async Task<ChatMessage> SendChat(ChatMessage message, HubConnection? hubConnection) 
        {
            if (hubConnection is not null && hubConnection.State == HubConnectionState.Connected)
            {
                // Save chat to DB
                await AddObject(message);
                await hubConnection.SendAsync("SendPrivateMessage", message);
            }

            return message;
        }
    }
}
