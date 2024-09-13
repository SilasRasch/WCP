using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCPShared.Models
{
    public class S3Settings
    {
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string ServiceUrl { get; set; } = string.Empty;
        public string Bucket { get; set; } = string.Empty;
        public string Root { get; set; } = string.Empty;
    }
}
