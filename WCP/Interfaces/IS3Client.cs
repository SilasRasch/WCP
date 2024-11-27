namespace WCPShared.Interfaces
{
    public interface IS3Client
    {
        Task<string> UploadImage(string fileName, Stream fileStream, string? fileType = "image/jpg");
        Task<string> UploadFile(string fileName, Stream fileStream, string MimeType);
    }
}
