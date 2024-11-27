namespace WCPShared.Interfaces
{
    public interface IS3Client
    {
        Task<string> UploadFile(string fileName, Stream fileStream, string mimeType);
    }
}
