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

        [HttpPost("profile-pic")]
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

        [HttpPost("template-img")]
        public async Task<ActionResult<Dictionary<string,string>>> UploadTemplateImages(List<IFormFile> files)
        {
            Dictionary<string, string> urls = new Dictionary<string, string>();
            foreach (var element in files.Select((File, Index) => (File, Index)))
            {
                string regexFileExtension = @"(png|jpe?g)";
                if (!Regex.IsMatch(Path.GetExtension(element.File.FileName).ToLower(), regexFileExtension) || !element.File.ContentType.ToLower().Contains("image"))
                    return BadRequest("File-type not accepted");

                if (element.File.Length > 1024 * 1024 * 10) // 10 Mb
                    return BadRequest("File size above limit");

                await using var memoryStream = new MemoryStream();
                await element.File.CopyToAsync(memoryStream);

                var fileExtension = Path.GetExtension(element.File.FileName);
                var userId = _userContextService.GetId();
                var fileName = $"statics/{userId}-{Guid.NewGuid()}{fileExtension.ToLower()}";

                // Set correct mime-type for jpegs
                if (fileExtension.ToLower() == ".jpg")
                    fileExtension = ".jpeg";
    
                string url = await _client.UploadImage(fileName, memoryStream, $"image/{fileExtension.Substring(1)}");

                switch (element.Index)
                {
                    case 0:
                        urls.Add("templateImgOne", url);
                        break;
                    case 1:
                        urls.Add("templateImgTwo", url);
                        break;
                    case 2:
                        urls.Add("exampleImg", url);
                        break;
                    default:
                        break;
                }
            }

            return Ok(urls);
        }
    }
}
