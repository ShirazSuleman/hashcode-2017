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
      var totalVideoSize = Videos.Sum(v => v.SizeInMb);

      if (totalVideoSize + video.SizeInMb > SizeInMb)
      {
        return false;
      }

      Videos.Add(video);
      return true;
    }
  }
}
