using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCPShared.Services.StaticHelpers
{
    public static class Secrets
    {
        public const string Issuer = "https://webcontent.dk";
        public const string Audience = "https://wcp.dk";
        public const string RefreshTokenCookieName = "_WCRefreshToken";

        public const string MongoCollectionName = "orders";
        public const string MongoCreatorCollectionName = "creators";
        public const string MongoBrandCollectionName = "brands";
        public const string MongoDBName = "webcontent";

        public static bool IsProd
        {
            get
            {
                if (Environment.GetEnvironmentVariable("WC_ENVIRONMENT") == "production")
                    return true;
                else return false;
            }
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
                env = Environment.GetEnvironmentVariable("TEST_CONNECTION_STRING")!;
                appsetting = config.GetSection("TestConnectionString").Value!;
            }


            if (env != null)
                return env;

            return appsetting;
        }

        public static string GetMongoConnectionString(IConfiguration config)
        {
            var env = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING")!;
            var appsetting = config.GetSection("MongoConnectionString").Value!;

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

        public static readonly string[] Origins = [
            "http://localhost",
            "http://localhost:5173",
            "https://wcp.dk",
            "https://test.wcp.dk",
            "http://172.232.142.14:8181",
            "http://172.232.142.14"
        ];
    }
}
