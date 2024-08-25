﻿using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.Extensions.Options;

namespace WCPFileAPI.Services.S3
{
    public class S3Client : IS3Client
    {
        private readonly S3Settings _settings;

        public S3Client(IOptionsMonitor<S3Settings> optionsMonitor)
        {
            _settings = optionsMonitor.CurrentValue;
        }

        public async Task<string> UploadImage(string fileName, Stream fileStream, string? fileType = "image/jpg")
        {
            using var client = _client;
            var request = new PutObjectRequest
            {
                BucketName = _settings.Bucket,
                Key = $"{_settings.Root}/{fileName}",
                ContentType = fileType,
                InputStream = fileStream,
                CannedACL = S3CannedACL.PublicRead,
            };
            await client.PutObjectAsync(request);
            return $"https://{_settings.Bucket}.{_settings.ServiceUrl}/{request.Key}";
        }

        private AmazonS3Client _client => new AmazonS3Client(
            _settings.AccessKey,
            _settings.SecretKey,
            new AmazonS3Config
            {
                ServiceURL = $"https://{_settings.ServiceUrl}/",
            }
        );
    }

    public class S3Settings
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string ServiceUrl { get; set; }
        public string Bucket { get; set; }
        public string Root { get; set; }
    }
}
