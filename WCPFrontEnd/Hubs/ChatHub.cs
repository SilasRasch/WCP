using Microsoft.AspNetCore.SignalR;
using WCPShared.Models.Entities.UserModels;

namespace WCPFrontEnd.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string userTo, User userFrom, string message)
        {
            await Clients.User(userTo).SendAsync("ReceiveMessage", userFrom, message);
        }
    }
}
