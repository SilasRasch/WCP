using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCPShared.Models.OrderModels
{
    public class Links
    {
        [BsonElement("scripts")]
        public string Scripts { get; set; } = string.Empty;
        [BsonElement("content")]
        public string Content { get; set; } = string.Empty;
        [BsonElement("other")]
        public string Other { get; set; } = string.Empty;
    }
}
