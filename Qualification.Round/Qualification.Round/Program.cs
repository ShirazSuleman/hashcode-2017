using Qualification.Round.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Qualification.Round
{
  class Program
  {
    static void Main(string[] args)
    {
      const string dataSetLocation = "../../DataSet/";
      const string resultLocation = "../../Results/";
      const string fileName = "kittens.in";

      // Input
      var controller = ReadInputDataSet($"{dataSetLocation}{fileName}");


      // Processing
      var validVideos = controller.Videos.Where(v => v.SizeInMb <= controller.Caches.First().SizeInMb).ToList();

      var videoRequests = controller.Requests.Join(validVideos, description => description.VideoId, video => video.ID,
        (description, video) => new { description.NumOfRequests, video.SizeInMb, VideoId = video.ID, description.FromEndPointId }).ToList();

      var endpointLatencies = new List<EndpointCacheLatency>();

      for (var i = 0; i < controller.EndPoints.Count(); i++)
      {
        var orderedCacheLatencyList = controller.EndPoints[i].CacheLatencyList.OrderBy(cll => cll.LatencyInMs).ToList();

        for (var j = 0; j < orderedCacheLatencyList.Count(); j++)
        {
          endpointLatencies.Add(new EndpointCacheLatency
          {
            CacheId = orderedCacheLatencyList[j].CacheId,
            EndpointId = controller.EndPoints[i].ID,
            LatencyInMs = orderedCacheLatencyList[j].LatencyInMs
          });
        }
      }

      var orderedVideoLatencyRequests = new List<VideoLatencyRequest>();

      var videoRequestsGroups = videoRequests.GroupBy(vr => vr.FromEndPointId);
      var endpointLatenciesGroups = endpointLatencies.GroupBy(el => el.EndpointId);

      foreach (var videoRequestsGroup in videoRequestsGroups)
      {
        var endpoints = endpointLatenciesGroups.FirstOrDefault(group => group.Key == videoRequestsGroup.Key)?.Select(elg => new { elg.EndpointId, elg.CacheId, elg.LatencyInMs });
        var videos = videoRequestsGroup.Select(vrg => new { vrg.NumOfRequests, vrg.SizeInMb, vrg.VideoId, vrg.FromEndPointId });

        if (endpoints == null)
          continue;

        var joinResult = videos.Join(endpoints, v => v.FromEndPointId, e => e.EndpointId, (v, e) => new VideoLatencyRequest
        {
          VideoId = v.VideoId,
          SizeInMb = v.SizeInMb,
          NumOfRequests = v.NumOfRequests,
          EndpointId = e.EndpointId,
          LatencyInMs = e.LatencyInMs,
          CacheId = e.CacheId
        });

        var result = joinResult.OrderByDescending(vlc => vlc.NumOfRequests)
          .ThenBy(vlc => vlc.LatencyInMs)
          .ThenBy(vlc => vlc.SizeInMb)
          .GroupBy(vlr => new { vlr.VideoId, vlr.EndpointId }, (key, value) => value.First())
          .ToList();

        orderedVideoLatencyRequests.AddRange(result);
      }

      for (var i = 0; i < orderedVideoLatencyRequests.Count(); i++)
      {
        var cacheServer = controller.Caches.First(cache => cache.ID == orderedVideoLatencyRequests[i].CacheId);
        var video = controller.Videos.First(v => v.ID == orderedVideoLatencyRequests[i].VideoId);

        if (cacheServer.SizeInMb < video.SizeInMb)
          continue;

        if (controller.Caches.All(c => c.SizeInMb < controller.Videos.Min(v => v.SizeInMb)))
          break;

        cacheServer.TryAdd(video);
      }

      // Output
      WriteResultToFile($"{resultLocation}Result{fileName}", controller);
    }

    public static StreamVideoController ReadInputDataSet(string fileName)
    {
      var dataSet = System.IO.File.ReadAllLines(fileName);

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
      foreach (var index in Enumerable.Range(0, numOfCaches))
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
          endPointIndex++;
          rowIndex++;
          continue;
        }

        rowIndex++;

        for (int i = 0; i < numOfConnectedCaches; i++)
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
      foreach (var i in Enumerable.Range(0, numOfRequests))
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
      return new StreamVideoController
      {
        Videos = videos,
        EndPoints = endPoints,
        Requests = requestDescriptions,
        Caches = caches
      };
    }

    public static void WriteResultToFile(string fileName, StreamVideoController controller)
    {
      var fileLines = new List<string>();

      var cachesInUse = controller.Caches.Count(c => c.Videos.Any());
      fileLines.Add(cachesInUse.ToString());

      foreach (var cacheServer in controller.Caches)
      {
        if (!cacheServer.Videos.Any())
          continue;

        var cacheInfo = $"{cacheServer.ID} ";
        foreach (var video in cacheServer.Videos)
        {
          cacheInfo += $"{video.ID} ";
        }
        fileLines.Add(cacheInfo);
      }

      System.IO.File.WriteAllLines(fileName, fileLines);
    }
  }
}
