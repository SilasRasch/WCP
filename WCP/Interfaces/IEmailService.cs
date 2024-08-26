using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.UserModels;

namespace WCPShared.Interfaces
{
    public interface IEmailService
    {
        Task<HttpStatusCode> SendRegistrationEmail(User user, string token);
        Task<HttpStatusCode> SendForgotPasswordEmail(User user, string token);
    }
}
