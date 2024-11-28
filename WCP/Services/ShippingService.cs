using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using WCPShared.Interfaces;
using WCPShared.Models.Entities;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Services
{
    public class ShippingService
    {
        private readonly HttpClient _httpClient;
        private readonly IEmailService _emailService;

        private dynamic DefaultSender = new
        {
            name = "WebContent ApS",
            attention = "Mathias C. Hansen",
            address1 = "Græsted Park 25",
            zipcode = "3230",
            city = "Græsted",
            country_code = "DK",
            email = "info@webcontent.dk",
            mobile = "26247871",
        };

        private dynamic DefaultReceiver = new
        {
            name = "Sille Hejselbak",
            address1 = "Florsgade 3, 1th",
            zipcode = "2200",
            city = "Nørrebro",
            country_code = "DK",
            email = "emilkanaris@yahoo.dk",
            mobile = "40220080"
        };

        public ShippingService(HttpClient httpClient, IEmailService emailService, IConfiguration config)
        {
            _httpClient = httpClient;
            _emailService = emailService;
        }

        public async Task<ShipmentResponse> CreateShipment(string reference)
        {
            var shipmentPayload = new
            {
                test_mode = !Secrets.IsProd,
                own_agreement = false,
                label_format = "a4_pdf",
                product_code = "GLSDK_HD",
                service_codes = "EMAIL_NT,SMS_NT",
                reference = reference,
                sender = DefaultSender,
                receiver = DefaultReceiver,
                parcels = new[]
                {
                    new { weight = 1000 }
                }
            };

            var jsonContent = JsonConvert.SerializeObject(shipmentPayload, Formatting.Indented);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("shipments", content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ShipmentResponse>(jsonResponse)!;
        }

        public async Task<Label> GetLabel(int shipmentId)
        {
            var response = await _httpClient.GetAsync($"shipments/{shipmentId}/labels");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Label>(jsonResponse)!;
        }

        public async Task SendShippingEmail(CreatorParticipation participation, string label)
        {
            await _emailService.SendShippingEmail(participation, label);
        }
    }

    public class Label
    {
        public string Base64 { get; set; } = string.Empty;
        public string File_Format { get; set; } = string.Empty;
    }

    public class ShipmentResponse
    {
        public int Id { get; set; }
        public List<Label> Labels { get; set; }
    }
}
