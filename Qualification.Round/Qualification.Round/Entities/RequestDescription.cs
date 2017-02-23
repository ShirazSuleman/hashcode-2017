using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qualification.Round.Entities
{
    public class RequestDescription
    {
        public int ID { get; set; }
        public int VideoId { get; set; }
        public long NumOfRequests { get; set; }
        public int FromEndPointId { get; set; }
    }
}
