using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WCPFileAPI.Services.S3;
using WCPShared.Services;
using System.Text.RegularExpressions;

namespace WCPFileAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IS3Client _client;
        private readonly UserContextService _userContextService;

        public FilesController(IS3Client client, UserContextService userContextService)
        {
            _client = client;
            _userContextService = userContextService;
        }

        [HttpPost]
        public async Task<ActionResult<string>> UploadProfilePicture(IFormFile file)
        {
            string regexFileExtension = @"(png|jpe?g)";
            if (!Regex.IsMatch(Path.GetExtension(file.FileName).ToLower(), regexFileExtension) || !file.ContentType.ToLower().Contains("image"))
                return BadRequest("File-type not accepted");

            if (file.Length > 1024 * 1024 * 10) // 10 Mb
                return BadRequest("File size above limit");

            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var fileExtension = Path.GetExtension(file.FileName);
            var userId = _userContextService.GetId();
            var fileName = $"profile-pics/{userId}-{Guid.NewGuid()}{fileExtension.ToLower()}";

            // Set correct mime-type for jpegs
            if (fileExtension.ToLower() == ".jpg")
                fileExtension = ".jpeg";

            return Ok(await _client.UploadImage(fileName, memoryStream, $"image/{fileExtension.Substring(1)}"));
        }
    }
}
