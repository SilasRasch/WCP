using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCPShared.Models.UserModels.CreatorModels
{
    public class CreatorHandles
    {
        [BsonElement("instagram")]
        public string? Instagram { get; set; }
        [BsonElement("facebook")]
        public string? Facebook { get; set; }
        [BsonElement("tikTok")]
        public string? TikTok { get; set; }
        [BsonElement("snapChat")]
        public string? Snapchat { get; set; }
        [BsonElement("pinterest")]
        public string? Pinterest { get; set; }
        [BsonElement("youTube")]
        public string? YouTube { get; set; }
    }
}
