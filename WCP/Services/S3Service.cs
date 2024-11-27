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
using WCPShared.Models.Entities.ProjectModels;

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

            var stream = file.OpenReadStream(maxAllowedSize: 1024 * 1024 * 10);

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

            var stream = file.OpenReadStream(maxAllowedSize: 1024 * 1024);

            var fileExtension = Path.GetExtension(file.Name);
            var fileName = $"statics/{Guid.NewGuid()}{fileExtension.ToLower()}";

            // Set correct mime-type for jpegs
            if (fileExtension.ToLower() == ".jpg")
                fileExtension = ".jpeg";

            return await _client.UploadImage(fileName, stream, $"image/{fileExtension.Substring(1)}");
        }

        public async Task<string> UploadCreatorContent(IBrowserFile file, Project project, int video, string subFolder) // Subfolder = Visuals/Voiceover
        {
            string regexFileExtension = @"(mp4|mov|avi|wmv)";
            if (!Regex.IsMatch(Path.GetExtension(file.Name).ToLower(), regexFileExtension) || !file.ContentType.ToLower().Contains("video"))
                throw new ArgumentException("File-type not accepted");

            var maxFileSize = 1024 * 1024 * 1024; // 1 TB
            if (file.Size > maxFileSize) 
                throw new ArgumentException("File size above limit");

            var stream = file.OpenReadStream(maxAllowedSize: maxFileSize);

            var fileExtension = Path.GetExtension(file.Name);
            var fileName = $"{project.Brand.Name}/{project.Id}/Content/{video}/{subFolder}/{file.Name}";

            return await _client.UploadFile(fileName, stream, $"video/{fileExtension.Substring(1)}");
        }

        public async Task<string> UploadFinalContent(IBrowserFile file, Project project, string format, int video) // Subfolder = Visuals/Voiceover
        {
            string regexFileExtension = @"(mp4|mov|avi|wmv)";
            if (!Regex.IsMatch(Path.GetExtension(file.Name).ToLower(), regexFileExtension) || !file.ContentType.ToLower().Contains("video"))
                throw new ArgumentException("File-type not accepted");

            var maxFileSize = 1024 * 1024 * 1024; // 1 TB
            if (file.Size > maxFileSize)
                throw new ArgumentException("File size above limit");

            var stream = file.OpenReadStream(maxAllowedSize: maxFileSize);

            var fileExtension = Path.GetExtension(file.Name);
            var fileName = $"{project.Brand.Name}/{project.Id}/Content/{format}/{video}.{fileExtension}";

            return await _client.UploadFile(fileName, stream, $"video/{fileExtension.Substring(1)}");
        }

        public async Task<string> UploadScript(IBrowserFile file, Project project)
        {
            string regexFileExtension = @"(docx)";
            if (!Regex.IsMatch(Path.GetExtension(file.Name).ToLower(), regexFileExtension) || !file.ContentType.ToLower().Contains("application/msword"))
                throw new ArgumentException("File-type not accepted");

            var maxFileSize = 1024 * 1024 * 64; // 64 mb
            if (file.Size > maxFileSize)
                throw new ArgumentException("File size above limit");

            var stream = file.OpenReadStream(maxAllowedSize: maxFileSize);

            var fileExtension = Path.GetExtension(file.Name);
            var fileName = $"{project.Brand.Name}/{project.Id}/Scripts/{file.Name}";

            return await _client.UploadFile(fileName, stream, $"application/msword");
        }
    }
}