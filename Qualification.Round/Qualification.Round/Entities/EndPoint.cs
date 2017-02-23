using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qualification.Round.Entities
{
    public class EndPoint
    {
        public int ID { get; set; }
        public long LatencyToDataCenterInMs { get; set; }
        //latency tp caches
        public List<CacheLatency> CacheLatencyList { get; set; }
    }
}
