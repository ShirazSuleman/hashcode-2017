using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qualification.Round.Entities
{
  public class CacheServer
  {
    public int ID { get; set; }
    public List<Video> Videos { get; set; }
    public int SizeInMb { get; set; }

    public bool TryAdd(Video video)
    {
      if (Videos.Any(v => v.ID == video.ID))
      {
        return false;
      }

      SizeInMb -= video.SizeInMb;

      Videos.Add(video);
      return true;
    }
  }
}
