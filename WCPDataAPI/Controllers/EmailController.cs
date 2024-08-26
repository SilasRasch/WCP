using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WCPShared.Interfaces;

namespace WCPDataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("report")]
        public async Task<IActionResult> Report(ReportInfo report)
        {
            var response = await _emailService.SendReportEmail(report.Email, report.Message);

            return response == HttpStatusCode.Accepted ? Ok() : BadRequest(response);
        }

        public class ReportInfo
        {
            public string Email { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;

        }
    }
}
