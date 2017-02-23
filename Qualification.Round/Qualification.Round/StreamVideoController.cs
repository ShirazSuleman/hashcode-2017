using Qualification.Round.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qualification.Round
{
    public class StreamVideoController
    {
        public List<Video> Videos { get; set; }
        public List<EndPoint> EndPoints { get; set; }
        public List<RequestDescription> Requests { get; set; }
        public List<CacheServer> Caches { get; set; }
    }
}
