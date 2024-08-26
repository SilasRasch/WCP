using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCPShared.Models.OrderModels
{
    public class Status
    {
        [BsonElement("category")]
        public int Category { get; set; } // 1: Queued -> 2: Planned -> 3: In progress -> 4: Feedback
        [BsonElement("state")]
        public int State { get; set; } // -1: Denied/Cancelled -> 0: Unconfirmed -> 1: Confirmed -> 2: Completed
    }
}
