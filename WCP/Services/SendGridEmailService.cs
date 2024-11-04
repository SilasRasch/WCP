using Microsoft.Extensions.Configuration;
using System.Net;
using WCPShared.Interfaces;
using WCPShared.Services.StaticHelpers;
using SendGrid.Helpers.Mail;
using SendGrid;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.UserModels;

namespace WCPShared.Services
{
    public class SendGridEmailService : IEmailService
    {
        private readonly string _wcEmail = Secrets.IsProd ? "info@webcontent.dk" : "udvikling@webcontent.dk";
        private readonly IConfiguration _configuration;

        public SendGridEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<HttpStatusCode> SendRegistrationEmail(User user, string token, bool selfRegister = false)
        {
            var apiKey = Secrets.GetSendGridAPI(_configuration);
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("info@webcontent.dk", "WebContent Platform"));
            msg.AddTo(new EmailAddress(user.Email, user.Name));
            msg.SetTemplateId("d-09b4d4101889434eb93492fd812ddaf4");

            string endpoint = selfRegister ? "register" : "verify";

            var dynamicTemplateDate = new
            {
                name = user.Name,
                link = Secrets.IsProd ? $"https://wcp.dk/{endpoint}?token={token}" : $"https://test.wcp.dk/{endpoint}?token={token}",
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
            msg.AddTo(new EmailAddress(user.Email, user.Name));
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

        public async Task<HttpStatusCode> SendNotificationEmail(string name, string email, string projectName, string projectCategory)
        {
            var apiKey = Secrets.GetSendGridAPI(_configuration);
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("info@webcontent.dk", "WebContent Platform"));
            msg.AddTo(new EmailAddress(email, name));
            msg.SetTemplateId("d-2cc539eb5bc54175b5ce09c826548760");

            msg.SetTemplateData(new { projectName, projectCategory, name });

            var response = await client.SendEmailAsync(msg);
            
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted)
                throw new ArgumentException($"Send Grid mail was not sent... {response.StatusCode} : {await response.Body.ReadAsStringAsync()}");

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> SendReportEmail(string email, string message)
        {
            var apiKey = Secrets.GetSendGridAPI(_configuration);
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("info@webcontent.dk", "WebContent Platform"));
            msg.AddTo(new EmailAddress("udvikling@webcontent.dk", "WebContent Info"));
            msg.SetTemplateId("d-390b09db349740fabff193b9a3064f23");

            msg.SetTemplateData(new { email, message });

            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted)
                throw new ArgumentException($"Send Grid mail was not sent... {response.StatusCode} : {await response.Body.ReadAsStringAsync()}");
            return response.StatusCode;
        }

        public async Task<HttpStatusCode> SendBrandCreationEmail(Brand brand)
        {
            var apiKey = Secrets.GetSendGridAPI(_configuration);
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("info@webcontent.dk", "WebContent Platform"));
            msg.AddTo(new EmailAddress(_wcEmail, "WebContent Info"));
            msg.SetTemplateId("d-a775face789d4c8388c5c6e5c81582bb");

            msg.SetTemplateData(new
            {
                brandName = brand.Name,
                brandURL = brand.URL,
                organizationId = brand.OrganizationId,
            });

            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted)
                throw new ArgumentException($"Send Grid mail was not sent... {response.StatusCode} : {await response.Body.ReadAsStringAsync()}");

            return response.StatusCode;
        }
    }
}
