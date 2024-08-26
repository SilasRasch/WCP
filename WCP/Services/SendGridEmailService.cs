using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Interfaces;
using WCPShared.Models.UserModels;
using WCPShared.Services.StaticHelpers;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace WCPShared.Services
{
    public class SendGridEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public SendGridEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<HttpStatusCode> SendRegistrationEmail(User user, string token)
        {
            var apiKey = Secrets.GetSendGridAPI(_configuration);
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("info@webcontent.dk", "WebContent Platform"));
            msg.AddTo(new EmailAddress(user.Email, user.DisplayName));
            msg.SetTemplateId("d-09b4d4101889434eb93492fd812ddaf4");

            var dynamicTemplateDate = new
            {
                name = user.DisplayName,
                link = Secrets.IsProd ? $"https://wcp.dk/verify?token={token}" : $"https://test.wcp.dk/verify?token={token}",
            };
            msg.SetTemplateData(dynamicTemplateDate);

            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted)
                throw new ArgumentException($"Send Grid mail was not sent... {response.StatusCode} : {await response.Body.ReadAsStringAsync()}");

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> SendForgotPasswordEmail(User user, string token)
        {
            var apiKey = Secrets.GetSendGridAPI(_configuration);
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("info@webcontent.dk", "WebContent Platform"));
            msg.AddTo(new EmailAddress(user.Email, user.DisplayName));
            msg.SetTemplateId("d-604ac0bfd5024dd1b7ee756414bc7847");

            var dynamicTemplateDate = new
            {
                link = Secrets.IsProd ? $"https://wcp.dk/reset-kodeord?token={token}" : $"https://test.wcp.dk/reset-kodeord?token={token}",
            };
            msg.SetTemplateData(dynamicTemplateDate);

            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted)
                throw new ArgumentException($"Send Grid mail was not sent... {response.StatusCode} : {await response.Body.ReadAsStringAsync()}");

            return response.StatusCode;
        }
    }
}
