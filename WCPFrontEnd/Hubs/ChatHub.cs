using Microsoft.AspNetCore.SignalR;

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

        public async Task SendPrivateMessage(string toUser, string message)
        {
            if (_userConnections.TryGetValue(toUser, out var connectionId))
            {
                var fromUser = _userConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
                if (!string.IsNullOrEmpty(fromUser))
                {
                    // Send the message to the recipient
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", fromUser, message);
                }
            }
            else
            {
                // Optionally notify sender that the recipient is offline
                await Clients.Caller.SendAsync("ReceiveMessage", "System", $"{toUser} is not online.");
            }
        }
    }
}
