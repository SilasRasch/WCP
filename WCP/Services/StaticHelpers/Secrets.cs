using Microsoft.Extensions.Configuration;
using SlackNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.Entities;

namespace WCPShared.Services.StaticHelpers
{
    public static class Secrets
    {
        public const string Issuer = "https://webcontent.dk";
        public const string Audience = "https://wcp.dk";
        public const string RefreshTokenCookieName = "_WCRefreshToken";

        public static bool IsProd
        {
            get
            {
                if (Environment.GetEnvironmentVariable("WC_ENVIRONMENT") == "production")
                    return true;
                else return false;
            }
        }

        public static string GetStripeApiKey(IConfiguration config)
        {
            var env = Environment.GetEnvironmentVariable("STRIPE_API_KEY")!;
            var appsetting = config.GetSection("StripeApiKey").Value!;

            if (env != null)
                return env;

            return appsetting;
        }

        public static string GetStripeWebhookKey(IConfiguration config)
        {
            var env = Environment.GetEnvironmentVariable("STRIPE_WH_KEY")!;
            var appsetting = config.GetSection("StripeWhKey").Value!;

            if (env != null)
                return env;

            return appsetting;
        }

        public static string GetConnectionString(IConfiguration config)
        {
            string env;
            string appsetting;

            if (IsProd)
            {
                env = Environment.GetEnvironmentVariable("CONNECTION_STRING")!;
                appsetting = config.GetSection("ConnectionString").Value!;
            }
            else
            {
                env = Environment.GetEnvironmentVariable("CONNECTION_STRING")!;
                appsetting = config.GetSection("ConnectionString").Value!;
            }


            if (env != null)
                return env;

            return appsetting;
        }

        public static string GetSendGridAPI(IConfiguration config)
        {
            var env = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")!;
            var appsetting = config.GetSection("SendGrid:SendGridAPI").Value!;

            if (env != null)
                return env;

            return appsetting;
        }

        public static string GetJwtKey(IConfiguration config)
        {
            var env = Environment.GetEnvironmentVariable("JWT_KEY")!;
            var appsetting = config.GetSection("Jwt:GeneratedToken").Value!;

            if (env != null)
                return env;

            return appsetting;
        }

        public static string GetShipmondoPassword(IConfiguration config)
        {
            var env = Environment.GetEnvironmentVariable("SHIPMONDO_PSWD")!;
            var appsetting = config.GetSection("ShipmondoPassword").Value!;

            if (env != null)
                return env;

            return appsetting;
        }

        public static IEnumerable<string> GetAudiences()
        {
            List<string> audiences = new List<string>(Origins);

            return audiences;
        }

        public static string GetSlackKey(IConfiguration config)
        {
            var env = Environment.GetEnvironmentVariable("SLACK_KEY")!;
            var appsetting = config.GetSection("SlackKey").Value!;

            if (env != null)
                return env;

            return appsetting;
        }

        public static S3Settings GetS3Settings(IConfiguration config)
        {
            if (IsProd)
            {
                return new S3Settings
                {
                    AccessKey = Environment.GetEnvironmentVariable("S3_ACCESS_KEY")!,
                    SecretKey = Environment.GetEnvironmentVariable("S3_SECRET_KEY")!,
                    ServiceUrl = Environment.GetEnvironmentVariable("S3_SERVICE_URL")!,
                    Bucket = Environment.GetEnvironmentVariable("S3_BUCKET")!,
                    Root = Environment.GetEnvironmentVariable("S3_ROOT")!,
                };
            }
            else
            {
                return new S3Settings
                {
                    AccessKey = config.GetSection("S3Settings:AccessKey").Value!,
                    SecretKey = config.GetSection("S3Settings:SecretKey").Value!,
                    ServiceUrl = config.GetSection("S3Settings:ServiceUrl").Value!,
                    Bucket = config.GetSection("S3Settings:Bucket").Value!,
                    Root = config.GetSection("S3Settings:Root").Value!
                };
            }
        }

        public static readonly string[] Origins = [
            "http://localhost",
            "http://localhost:5173",
            "https://wcp.dk",
            "https://www.wcp.dk",
            "https://test.wcp.dk",
            "http://172.232.142.14:8181",
            "http://172.232.142.14",
            "https://app.nobitches.win"
        ];

        public static Uri OtlpEndpoint => new Uri("http://localhost:4317"); 
    }
}
