using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qualification.Round.Entities
{
  public class VideoLatencyRequest
  {
    public int CacheId { get; internal set; }
    public int EndpointId { get; internal set; }
    public long LatencyInMs { get; internal set; }
    public long NumOfRequests { get; internal set; }
    public int SizeInMb { get; internal set; }
    public int VideoId { get; internal set; }
  }
}
