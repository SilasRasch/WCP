using System.Net;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.UserModels;

namespace WCPShared.Interfaces
{
    public interface IEmailService
    {
        Task<HttpStatusCode> SendRegistrationEmail(User user, string token, bool selfRegister = false);
        Task<HttpStatusCode> SendForgotPasswordEmail(User user, string token);
        Task<HttpStatusCode> SendNotificationEmail(string name, string email, string projectName, string projectCategory);
        Task<HttpStatusCode> SendReportEmail(string email, string message);
        Task<HttpStatusCode> SendBrandCreationEmail(Brand brand);
        Task<HttpStatusCode> SendShippingEmail(CreatorParticipation participation, string Base64Pdf);
    }
}
