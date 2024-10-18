using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WCPShared.Interfaces;
using WCPShared.Models;

namespace WCPShared.Services
{
    public class S3Service
    {
        private readonly IS3Client _client;

        public S3Service( IS3Client client)
        {
            _client = client;
        }

        public async Task<string> UploadProfilePicture(IBrowserFile file)
        {
            string regexFileExtension = @"(png|jpe?g)";
            if (!Regex.IsMatch(Path.GetExtension(file.Name).ToLower(), regexFileExtension) || !file.ContentType.ToLower().Contains("image"))
                throw new ArgumentException("File-type not accepted");

            if (file.Size > 1024 * 1024 * 10) // 10 Mb
                throw new ArgumentException("File size above limit");

            var stream = file.OpenReadStream();

            var fileExtension = Path.GetExtension(file.Name);
            var fileName = $"profile-pics/{Guid.NewGuid()}{fileExtension.ToLower()}";

            // Set correct mime-type for jpegs
            if (fileExtension.ToLower() == ".jpg")
                fileExtension = ".jpeg";

            return await _client.UploadImage(fileName, stream, $"image/{fileExtension.Substring(1)}");
        }

        public async Task<string> UploadStaticTemplateImage(IBrowserFile file)
        {
            string regexFileExtension = @"(png|jpe?g)";
            if (!Regex.IsMatch(Path.GetExtension(file.Name).ToLower(), regexFileExtension) || !file.ContentType.ToLower().Contains("image"))
                throw new ArgumentException("File-type not accepted");

            if (file.Size > 1024 * 1024 * 10) // 10 Mb
                throw new ArgumentException("File size above limit");

            var stream = file.OpenReadStream();

            var fileExtension = Path.GetExtension(file.Name);
            var fileName = $"statics/{Guid.NewGuid()}{fileExtension.ToLower()}";

            // Set correct mime-type for jpegs
            if (fileExtension.ToLower() == ".jpg")
                fileExtension = ".jpeg";

            return await _client.UploadImage(fileName, stream, $"image/{fileExtension.Substring(1)}");
        }
    }
}
