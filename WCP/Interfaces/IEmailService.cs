using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WCPShared.Models.BrandModels;
using WCPShared.Models.UserModels;

namespace WCPShared.Interfaces
{
    public interface IEmailService
    {
        Task<HttpStatusCode> SendRegistrationEmail(User user, string token);
        Task<HttpStatusCode> SendForgotPasswordEmail(User user, string token);
        Task<HttpStatusCode> SendNotificationEmail(string name, string email, string projectName, string projectCategory);
        Task<HttpStatusCode> SendReportEmail(string email, string message);
        Task<HttpStatusCode> SendBrandCreationEmail(Brand brand, string email);
    }
}
