namespace WCPFileAPI.Services.S3
{
    public interface IS3Client
    {
        Task<string> UploadImage(string fileName, Stream fileStream, string? fileType = "image/jpg");
    }
}
