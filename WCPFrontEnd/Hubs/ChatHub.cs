using Microsoft.AspNetCore.SignalR;
using WCPShared.Models.Entities;

namespace WCPFrontEnd.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly Dictionary<string, string> _userConnections = new();

        public override Task OnConnectedAsync()
        {
            var userName = Context.GetHttpContext()?.Request.Query["user"];
            if (!string.IsNullOrEmpty(userName))
            {
                _userConnections[userName] = Context.ConnectionId;
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            // Remove the user's connection
            var user = _userConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (user != null)
            {
                _userConnections.Remove(user);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendPrivateMessage(ChatMessage message)
        {
            if (_userConnections.TryGetValue(message.To.Email, out var connectionId))
            {
                var fromUser = _userConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
                if (!string.IsNullOrEmpty(fromUser))
                {
                    // Send the message to the recipient
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
                }
            }
        }
    }
}
