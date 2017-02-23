namespace Qualification.Round.Entities
{
  public class EndpointCacheLatency
  {
    public int CacheId { get; set; }
    public int EndpointId { get; set; }
    public long LatencyInMs { get; set; }
  }
}