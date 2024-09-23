using SlackNet;
using SlackNet.WebApi;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models;
using WCPShared.Models.Views;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Services
{
    public class SlackNotificationService
    {
        private readonly ISlackApiClient _slackApiClient;
        private readonly ICreatorService _creatorService;
        private readonly IUserService _userService;
        private bool EnableNotifications = Secrets.IsProd;

        public SlackNotificationService(ISlackApiClient slackApiClient, ICreatorService creatorService, IUserService userService)
        {
            _slackApiClient = slackApiClient;
            _creatorService = creatorService;
            _userService = userService;
        }

        public async Task SendMessageToUser(string username, string message)
        {
            if (!EnableNotifications) return;

            var user = await _userService.GetObjectBy(x => x.Name == username);
            if (user is null || !user.NotificationsOn || user.NotificationSetting !=  "slack") return;

            User? slackUser = await FetchUser(username);
            if (slackUser is not null)
                await SendMessage(slackUser.Id, message);
        }

        public async Task SendMessageToChannel(string channel, string message)
        {
            if (!EnableNotifications) return;

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

            return conversations.Channels.SingleOrDefault(x => x.Name == conversation.Replace(' ', '-').ToLower());
        }

        private async Task<User?> FetchUser(string userName)
        {
            return (await _slackApiClient.Users.List()).Members.SingleOrDefault(x => x.RealName.ToLower() == userName.ToLower());
        }

        public async Task SendStatusNotifications(Order newOrder, Order oldOrder)
        {
            if (!EnableNotifications) return;

            // Organizational notifications

            // Notify organization when the order is accepted
            if (newOrder.Status == 1 && oldOrder.Status == 0)
                await SendMessageToChannel(
                    newOrder.Brand.Organization.Name,
                    $"[{newOrder.ProjectName}] Tak for din bestilling - den er nu bekræftet! {InsertProjectLink(newOrder.Id)}");

            // Notify the organization when the scripts and creators have been choosen
            if (newOrder.Status == 2 && oldOrder.Status == 1)
                await SendMessageToChannel(
                    newOrder.Brand.Organization.Name,
                    $"[{newOrder.ProjectName}] Scripts og creators er nu klar - hop ind og accepter! {InsertProjectLink(newOrder.Id)}");

            // Notify the organization when the project has wrapped up production
            if (newOrder.Status == 5 && oldOrder.Status == 4)
                await SendMessageToChannel(
                    newOrder.Brand.Organization.Name,
                    $"[{newOrder.ProjectName}] Dit projekt er nu færdigt - hop ind og giv feedback! {InsertProjectLink(newOrder.Id)}");

            // Notify the organization when the project has been cancelled
            if (newOrder.Status == -1 && oldOrder.Status != -1)
                await SendMessageToChannel(
                    newOrder.Brand.Organization.Name,
                    $"[{newOrder.ProjectName}] Dit projekt er blevet annulleret {InsertProjectLink(newOrder.Id)}");

            // Creator notifications

            var allCreators = await _creatorService.GetAllObjectsView();
            IEnumerable<CreatorView> creatorsWithUsers = from Creator in newOrder.Creators
                                                  join CreatorView in allCreators
                                                  on Creator.Id equals CreatorView.Id
                                                  select CreatorView;

            // Notify creators when the project is moved from planned to production
            if (newOrder.Status == 3 && oldOrder.Status == 2)
                foreach (CreatorView creator in creatorsWithUsers)
                    await SendMessageToUser(
                        creator.User.Name,
                        $"[{newOrder.ProjectName}] Projektet er nu godkendt og produkterne er på vej til dig! {InsertProjectLink(newOrder.Id)}");

            // Notify creators when the project is moved from queued to planned
            if (newOrder.Status == 2 && oldOrder.Status == 1)
                foreach (CreatorView creator in creatorsWithUsers)
                    await SendMessageToUser(
                        creator.User.Name,
                        $"[{newOrder.ProjectName}] Du er blevet inviteret til et projekt! {InsertProjectLink(newOrder.Id)}");

            // Notify newly invited creators (only in planned ???)
            if (newOrder.Status == 2)
            {
                var newCreators = creatorsWithUsers.ExceptBy(oldOrder.Creators.Select(x => x.Id), x => x.Id);
                if (newCreators.Any())
                    foreach (CreatorView creator in newCreators)
                        await SendMessageToUser(
                            creator.User.Name,
                            $"[{newOrder.ProjectName}] Du er blevet inviteret til et projekt! {InsertProjectLink(newOrder.Id)}");
            }
        }

        private string InsertProjectLink(int id)
        {
            return $"<https://wcp.dk/projekt/{id}|Gå til projekt>";
        }
    }
}
