using SlackNet;
using SlackNet.WebApi;

namespace WCPShared.Services
{
    public class SlackNotificationService
    {
        private readonly ISlackApiClient _slackApiClient;

        public SlackNotificationService(ISlackApiClient slackApiClient)
        {
            _slackApiClient = slackApiClient;
        }

        public async Task SendMessageToUser(string username, string message)
        {
            User? user = await FetchUser(username);

            if (user is not null)
                await SendMessage(user.Id, message);
        }

        public async Task SendMessageToChannel(string channel, string message)
        {
            Conversation? conversation = await FetchConversation(channel);
            
            if (conversation is not null)
                await SendMessage(conversation.Id, message);
        }

        private async Task SendMessage(string conversationId, string message)
        {
            await _slackApiClient.Chat.PostMessage(new Message
            {
                Text = message,
                Channel = conversationId
            });
        }

        private async Task<Conversation?> FetchConversation(string conversation)
        {
            ConversationListResponse conversations = await _slackApiClient.Conversations.List(
                types: [ConversationType.PublicChannel, ConversationType.PrivateChannel]);

            return conversations.Channels.SingleOrDefault(x => x.Name == conversation);
        }

        private async Task<User?> FetchUser(string userName)
        {
            return (await _slackApiClient.Users.List()).Members.SingleOrDefault(x => x.RealName == userName);
        }
    }
}
