using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using WCPShared.Interfaces;
using WCPShared.Models.Entities;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Models
{
    public class S3Client : IS3Client
    {
        private readonly S3Settings _settings;

        public S3Client(IConfiguration configuration)
        {
            _settings = Secrets.GetS3Settings(configuration);
        }

        public async Task<string> UploadFile(string fileName, Stream fileStream, string mimeType)
        {
            using var client = _client;
            var request = new PutObjectRequest
            {
                BucketName = _settings.Bucket,
                Key = $"{_settings.Root}/{fileName}",
                ContentType = mimeType,
                InputStream = fileStream,
                CannedACL = S3CannedACL.PublicRead,
            };
            await client.PutObjectAsync(request);
            return $"https://{_settings.Bucket.ToLower()}.{_settings.ServiceUrl}/{request.Key}";
        }

        public async Task DeleteFile(string key)
        {
            using var client = _client;
            if (key.StartsWith(BucketBase))
            {
                key = key.Substring(BucketBase.Length);
            }

            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _settings.Bucket,
                Key = key
            };

            var res = await client.DeleteObjectAsync(deleteObjectRequest);
        }

        public async Task<string> CopyFileAsync(string sourceKey, string destinationKey)
        {
            if (sourceKey == destinationKey)
                return null;
            
            using var client = _client;
            if (sourceKey.StartsWith(BucketBase))
            {
                sourceKey = sourceKey.Substring(BucketBase.Length);
            }

            if (destinationKey.StartsWith(BucketBase))
            {
                destinationKey = destinationKey.Substring(BucketBase.Length);
            }

            var copyRequest = new CopyObjectRequest
            {
                SourceBucket = _settings.Bucket,
                SourceKey = sourceKey,
                DestinationBucket = _settings.Bucket,
                DestinationKey = destinationKey,
                CannedACL = S3CannedACL.PublicRead,
            };

            await client.CopyObjectAsync(copyRequest);
            return $"https://{_settings.Bucket.ToLower()}.{_settings.ServiceUrl}/{destinationKey}";
        }

        private AmazonS3Client _client => new AmazonS3Client(
            _settings.AccessKey,
            _settings.SecretKey,
            new AmazonS3Config
            {
                ServiceURL = $"https://{_settings.ServiceUrl}/",
            }
        );

        private string BucketBase => $"https://{_settings.Bucket}.{_settings.ServiceUrl}/";
    }
}
