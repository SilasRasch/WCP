using Microsoft.AspNetCore.Components.Forms;
using System.Text.RegularExpressions;
using WCPShared.Interfaces;
using WCPShared.Models.Entities.ProjectModels;

namespace WCPShared.Services
{
    public class S3Service
    {
        private readonly IS3Client _client;

        public S3Service(IS3Client client)
        {
            _client = client;
        }

        private readonly string videoExtensions = @"(mp4|mov|avi|wmv|mkv|flv)";
        private readonly string audioExtensions = @"(mp3|wav|wma|aac|oga|webm|m4a|aiff|flac|alac)";
        private readonly string imageExtensions = @"(png|jpe?g)";

        public async Task<string> UploadProfilePicture(IBrowserFile file)
        {
            string regexFileExtension = imageExtensions;
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

            return await _client.UploadFile(fileName, stream, file.ContentType);
        }

        public async Task<string> UploadStaticTemplateImage(IBrowserFile file)
        {
            string regexFileExtension = imageExtensions;
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

            return await _client.UploadFile(fileName, stream, file.ContentType);
        }

        public async Task<string> UploadCreatorVideo(IBrowserFile file, Project project, string contentName, string subFolder) // Subfolder = Visuals/Voiceover
        {
            string regexFileExtension = videoExtensions;
            if (!Regex.IsMatch(Path.GetExtension(file.Name).ToLower(), regexFileExtension) || !file.ContentType.ToLower().Contains("video"))
                throw new ArgumentException("File-type not accepted");

            var maxFileSize = 1024 * 1024 * 1024; // 1 GB
            if (file.Size > maxFileSize) 
                throw new ArgumentException("File size above limit");

            var stream = file.OpenReadStream(maxAllowedSize: maxFileSize);

            var fileExtension = Path.GetExtension(file.Name);
            var fileName = $"{project.Brand.Name}/{project.Id}/Content/{contentName}/{subFolder}/{file.Name}";

            return await _client.UploadFile(fileName, stream, file.ContentType);
        }

        public async Task<string> UploadCreatorVoiceover(IBrowserFile file, Project project, string contentName, string subFolder) // Subfolder = Visuals/Voiceover
        {
            string regexFileExtension = audioExtensions;
            if (!Regex.IsMatch(Path.GetExtension(file.Name).ToLower(), regexFileExtension) || !file.ContentType.ToLower().Contains("audio"))
                throw new ArgumentException("File-type not accepted");

            var maxFileSize = 1024 * 1024 * 1024; // 1 GB
            if (file.Size > maxFileSize)
                throw new ArgumentException("File size above limit");

            var stream = file.OpenReadStream(maxAllowedSize: maxFileSize);

            var fileExtension = Path.GetExtension(file.Name);
            var fileName = $"{project.Brand.Name}/{project.Id}/Content/{contentName}/{subFolder}/{file.Name}";

            return await _client.UploadFile(fileName, stream, file.ContentType);
        }

        public async Task<string> UploadCreatorImage(IBrowserFile file, Project project, string contentName, string subFolder)
        {
            string regexFileExtension = imageExtensions;
            if (!Regex.IsMatch(Path.GetExtension(file.Name).ToLower(), regexFileExtension) || !file.ContentType.ToLower().Contains("video"))
                throw new ArgumentException("File-type not accepted");

            var maxFileSize = 1024 * 1024 * 64; // 64 MB
            if (file.Size > maxFileSize)
                throw new ArgumentException("File size above limit");

            var stream = file.OpenReadStream(maxAllowedSize: maxFileSize);

            var fileExtension = Path.GetExtension(file.Name);
            var fileName = $"{project.Brand.Name}/{project.Id}/Content/{contentName}/{subFolder}/{file.Name}";

            return await _client.UploadFile(fileName, stream, file.ContentType);
        }

        public async Task<string> UploadCloudFile(IBrowserFile file, string path)
        {
            var maxFileSize = (long) Math.Pow(1024, 3) * 8; // 8 GB
            
            if (file.Size > maxFileSize)
                throw new ArgumentException("File size above limit");

            var stream = file.OpenReadStream(maxAllowedSize: maxFileSize);

            var fileExtension = Path.GetExtension(file.Name);
            var fileName = $"{path}/{file.Name}";

            return await _client.UploadFile(fileName, stream, file.ContentType);
        }

        public async Task<string> UploadFinalContent(IBrowserFile file, Project project, string format, int video) // Subfolder = Visuals/Voiceover
        {
            string regexFileExtension = videoExtensions;
            if (!Regex.IsMatch(Path.GetExtension(file.Name).ToLower(), regexFileExtension) || !file.ContentType.ToLower().Contains("video"))
                throw new ArgumentException("File-type not accepted");

            var maxFileSize = 1024 * 1024 * 1024; // 1 TB
            if (file.Size > maxFileSize)
                throw new ArgumentException("File size above limit");

            var stream = file.OpenReadStream(maxAllowedSize: maxFileSize);

            var fileExtension = Path.GetExtension(file.Name);
            var fileName = $"{project.Brand.Name}/{project.Id}/Content/{format}/{video}.{fileExtension}";

            return await _client.UploadFile(fileName, stream, file.ContentType);
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

            return await _client.UploadFile(fileName, stream, file.ContentType);
        }

        public async Task<string> UploadOtherFile(IBrowserFile file, Project project)
        {
            var maxFileSize = 1024 * 1024 * 1024; // 1 TB
            if (file.Size > maxFileSize)
                throw new ArgumentException("File size above limit");

            var stream = file.OpenReadStream(maxAllowedSize: maxFileSize);

            var fileExtension = Path.GetExtension(file.Name);
            var fileName = $"{project.Brand.Name}/{project.Id}/Other/{file.Name}";

            return await _client.UploadFile(fileName, stream, file.ContentType);
        }

        public async Task DeleteFile(string fullUri)
        {
            await _client.DeleteFile(fullUri);
        }

        public async Task<string> RenameFile(string sourceUri, string newFileName) 
        {
            string newKey = sourceUri.Replace(sourceUri.Split('/').Last(), newFileName) + Path.GetExtension(sourceUri);
            string newUri = await _client.CopyFileAsync(sourceUri, newKey);
            
            if (!string.IsNullOrEmpty(newUri))
                await _client.DeleteFile(sourceUri);

            return newUri;
        }
    }
}