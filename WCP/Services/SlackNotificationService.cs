using SlackNet;
using SlackNet.WebApi;
using WCPShared.Interfaces.DataServices;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.ProjectModels;
using WCPShared.Models.Enums;
using WCPShared.Models.Views;
using WCPShared.Services.EntityFramework;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Services
{
    public class SlackNotificationService
    {
        private readonly ISlackApiClient _slackApiClient;
        private readonly CreatorService _creatorService;
        private readonly UserService _userService;
        private bool EnableNotifications = Secrets.IsProd;

        public SlackNotificationService(ISlackApiClient slackApiClient, CreatorService creatorService, UserService userService)
        {
            _slackApiClient = slackApiClient;
            _creatorService = creatorService;
            _userService = userService;
        }

        public async Task SendMessageToUser(string username, string message)
        {
            if (!EnableNotifications) return;

            var user = await _userService.GetObjectBy(x => x.Name == username);
            if (user is null || !user.NotificationsOn || user.NotificationSetting.ToLower() !=  "slack") return;

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
            return (await _slackApiClient.Users.List()).Members.SingleOrDefault(x => x.RealName == userName);
        }

        public async Task SendStatusNotifications(Project newProject, Project oldProject)
        {
            if (!EnableNotifications) return;

            // Organizational notifications

            // Notify organization when the project is accepted
            if (newProject.Status == ProjectStatus.Queued && oldProject.Status == ProjectStatus.Unconfirmed)
                await SendMessageToChannel(
                    newProject.Brand.Organization.Name,
                    $"[{newProject.Name}] Tak for din bestilling - den er nu bekræftet! {InsertProjectLink(newProject.Id)}");

            // Notify the organization when the scripts and creators have been choosen
            if (newProject.Status == ProjectStatus.Planned && oldProject.Status == ProjectStatus.Scripting)
                await SendMessageToChannel(
                    newProject.Brand.Organization.Name,
                    $"[{newProject.Name}] Scripts og creators er nu klar - hop ind og accepter! {InsertProjectLink(newProject.Id)}");

            // Notify the organization when the project has wrapped up production
            if (newProject.Status == ProjectStatus.Feedback && oldProject.Status == ProjectStatus.Editing)
                await SendMessageToChannel(
                    newProject.Brand.Organization.Name,
                    $"[{newProject.Name}] Dit projekt er nu færdigt - hop ind og giv feedback! {InsertProjectLink(newProject.Id)}");

            // Notify the organization when the project has been cancelled
            if (newProject.Status == ProjectStatus.Cancelled && oldProject.Status != ProjectStatus.Cancelled)
                await SendMessageToChannel(
                    newProject.Brand.Organization.Name,
                    $"[{newProject.Name}] Dit projekt er blevet annulleret {InsertProjectLink(newProject.Id)}");

            // Creator notifications

            if (newProject is CreatorProject newCreatorProject && oldProject is CreatorProject oldCreatorProject)
            {
                var allCreators = await _creatorService.GetAllObjectsView();
                IEnumerable<CreatorView> creatorsWithUsers = from CreatorParticipation in newCreatorProject.Participations
                                                             join CreatorView in allCreators
                                                             on CreatorParticipation.Creator.Id equals CreatorView.Id
                                                             select CreatorView;

                // Notify creators when the project is moved from planned to production
                if (newCreatorProject.Status == ProjectStatus.CreatorFilming && oldCreatorProject.Status == ProjectStatus.Planned)
                    foreach (CreatorView creator in creatorsWithUsers)
                        await SendMessageToUser(
                            creator.User.Name,
                            $"[{newCreatorProject.Name}] Projektet er nu godkendt og produkterne er på vej til dig! {InsertProjectLink(newCreatorProject.Id)}");

                // Notify creators when the project is moved from scripting to planned
                if (newCreatorProject.Status == ProjectStatus.Planned && oldCreatorProject.Status == ProjectStatus.Scripting)
                    foreach (CreatorView creator in creatorsWithUsers)
                        await SendMessageToUser(
                            creator.User.Name,
                            $"[{newCreatorProject.Name}] Du er blevet inviteret til et projekt! {InsertProjectLink(newCreatorProject.Id)}");

                // Notify newly invited creators (only in planned ???)
                if (newCreatorProject.Status == ProjectStatus.Planned)
                {
                    var newCreators = creatorsWithUsers.ExceptBy(oldCreatorProject.Participations.Select(x => x.CreatorId), x => x.Id);
                    if (newCreators.Any())
                        foreach (CreatorView creator in newCreators)
                            await SendMessageToUser(
                                creator.User.Name,
                                $"[{newCreatorProject.Name}] Du er blevet inviteret til et projekt! {InsertProjectLink(newCreatorProject.Id)}");
                }
            }
        }

        private string InsertProjectLink(int id)
        {
            return $"<https://wcp.dk/projekt/{id}|Gå til projekt>";
        }
    }
}
