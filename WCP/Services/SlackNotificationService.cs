using SlackNet;
using SlackNet.WebApi;
using WCPShared.Models;

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

        public async Task SendStatusNotifications(Order newOrder, Order oldOrder)
        {
            // Organizational notifications

            if (newOrder.Status == 1 && oldOrder.Status == 0)
                await SendMessageToChannel(
                    newOrder.Brand.Organization.Name,
                    $"[{newOrder.ProjectName}] Tak for din bestilling - den er nu bekræftet!");

            if (newOrder.Status == 2 && oldOrder.Status == 1)
                await SendMessageToChannel(
                    newOrder.Brand.Organization.Name,
                    $"[{newOrder.ProjectName}] Scripts og creators er nu klar - hop ind og accepter!");

            if (newOrder.Status == 4 && oldOrder.Status == 5)
                await SendMessageToChannel(
                    newOrder.Brand.Organization.Name,
                    $"[{newOrder.ProjectName}] Scripts og creators er nu klar - hop ind og accepter!");

            if (newOrder.Status == -1 && oldOrder.Status != -1)
                await SendMessageToChannel(
                    newOrder.Brand.Organization.Name,
                    $"[{newOrder.ProjectName}] Scripts og creators er nu klar - hop ind og accepter!");

            // Creator notifications

            if (newOrder.Status == 3 && oldOrder.Status == 2)
                foreach (WCPShared.Models.UserModels.Creator creator in newOrder.Creators)
                    await SendMessageToUser(
                        creator.User.Name,
                        $"[{newOrder.ProjectName}] Projektet er nu godkendt og produkterne er på vej til dig!");

            var newCreators = newOrder.Creators.Except(oldOrder.Creators);

            if (newCreators.Any())
            {
                foreach (WCPShared.Models.UserModels.Creator creator in newCreators)
                    await SendMessageToUser(
                        creator.User.Name,
                        "Du er blevet inviteret til et projekt!");
            }
        }
    }
}
