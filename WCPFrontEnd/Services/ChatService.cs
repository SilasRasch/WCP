using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WCPFrontEnd.Components.Pages.Users;
using WCPFrontEnd.Models;
using WCPShared.Interfaces;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.UserModels;

namespace WCPFrontEnd.Services
{
    public class ChatService
    {
        private IWcpDbContext _context;

        public ChatService(IWcpDbContext context)
        {
            _context = context;
        }

        public async Task<ChatMessage> SendChat(ChatMessage message, HubConnection? hubConnection) 
        {
            if (hubConnection is not null && hubConnection.State == HubConnectionState.Connected)
            {
                // Save chat to DB
                await _context.AddAsync(message);
                await _context.SaveChangesAsync();
                await hubConnection.SendAsync("SendPrivateMessage", message);
            }

            return message;
        }

        public async Task<List<ChatMessage>> GetChatsByUsers(User user1, User user2)
        {
            return await _context.Chats
                .Include(x => x.From)
                .Include(x => x.To)
                .Where(x => (x.From.Id == user1.Id && x.To.Id == user2.Id) || (x.From.Id == user2.Id && x.To.Id == user1.Id))
                .OrderBy(x => x.Sent)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserWithLatestMessage>> GetUsersWithLatestChat(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users
            .Where(predicate)
            .Select(user => new UserWithLatestMessage
            {
                User = user,
                LatestMessage = _context.Chats
                    .Where(m => m.From.Id == user.Id || m.To.Id == user.Id) // Filter for messages related to the user
                    .OrderByDescending(m => m.Sent)                         // Order by latest
                    .FirstOrDefault()                                       // Take the latest message
            })
            .OrderByDescending(u => u.LatestMessage != null ? u.LatestMessage.Sent : DateTime.MinValue) // Sort by Timestamp
            .ToListAsync();
        }
    }
}