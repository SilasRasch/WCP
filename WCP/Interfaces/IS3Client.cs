namespace WCPShared.Interfaces
{
    public interface IS3Client
    {
        Task<string> UploadFile(string fileName, Stream fileStream, string mimeType);
        Task DeleteFile(string fileName);
        Task<string> CopyFileAsync(string sourceKey, string destinationKey);
    }
}
