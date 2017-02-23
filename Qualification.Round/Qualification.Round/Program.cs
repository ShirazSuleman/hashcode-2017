using Qualification.Round.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qualification.Round
{
  class Program
  {
    static void Main(string[] args)
    {
        var dataSet = System.IO.File.ReadAllLines("../../DataSet/me_at_the_zoo.in");

            var firstLine = dataSet[0].Split(' ');

            var numOfVideos = int.Parse(firstLine[0]);
            var numOfEndPoints = int.Parse(firstLine[1]);
            var numOfRequests = int.Parse(firstLine[2]);
            var numOfCaches = int.Parse(firstLine[3]);
            var cacheSize = int.Parse(firstLine[4]);

            //Process videos
            var videos = new List<Video>(numOfVideos);
            var videoSizes = dataSet[1].Split(' ');

            foreach (var index in Enumerable.Range(0, numOfVideos))
            {
                videos.Add(new Video
                {
                    ID = index,
                    SizeInMb = int.Parse(videoSizes[index])
                });
            }

            var caches = new List<CacheServer>();
            foreach(var index in Enumerable.Range(0, numOfCaches))
            {
                caches.Add(new CacheServer
                {
                    ID = index,
                    SizeInMb = cacheSize,
                    Videos = new List<Video>()
                });
            }

            var endPoints = new List<EndPoint>(numOfEndPoints);
            var rowIndex = 2;

            var endPointIndex = 0;
            while (endPointIndex < numOfEndPoints)
            {
                var endPointInfo = dataSet[rowIndex].Split(' ');
                var endPointLatency = long.Parse(endPointInfo[0]);
                var numOfConnectedCaches = int.Parse(endPointInfo[1]);

                var _endPoint = new EndPoint
                {
                    ID = endPointIndex,
                    LatencyToDataCenterInMs = endPointLatency,
                    CacheLatencyList = new List<CacheLatency>()
                };

                if (numOfConnectedCaches == 0)
                {
                    endPoints.Add(_endPoint);
                    rowIndex++;
                    continue;
                }

                rowIndex++;

                for(int i = 0; i < numOfConnectedCaches; i++)
                {
                    var cacheLatencyInfo = dataSet[rowIndex].Split(' ');
                    var cacheId = int.Parse(cacheLatencyInfo[0]);
                    var cacheLatency = long.Parse(cacheLatencyInfo[1]);
                    var cacheLatencyEntity = new CacheLatency
                    {
                        CacheId = cacheId,
                        LatencyInMs = cacheLatency
                    };
                    _endPoint.CacheLatencyList.Add(cacheLatencyEntity);

                    if (i + 1 < numOfConnectedCaches)
                        rowIndex++;
                }

                endPoints.Add(_endPoint);
                endPointIndex++;
                rowIndex++;
            }

            var requestDescriptions = new List<RequestDescription>(numOfRequests);
            foreach (var i in Enumerable.Range(0,numOfRequests))
            {
                var request = dataSet[rowIndex + i].Split(' ');
                
                var videoId = int.Parse(request[0]);
                var endPointId = int.Parse(request[1]);
                var numOfVideoRequests = int.Parse(request[2]);

                requestDescriptions.Add(new RequestDescription
                {
                    VideoId = videoId,
                    FromEndPointId = endPointId,
                    NumOfRequests = numOfVideoRequests
                });
            }
    }
  }
}
