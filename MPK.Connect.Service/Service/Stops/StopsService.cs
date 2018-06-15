using MPK.Connect.Model;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;

namespace MPK.Connect.Service.Service.Stops
{
    public class StopsService : IStopsService
    {
        private readonly ILogger<StopsService> _logger;
        private readonly IStopsRepository _stopsRepository;

        public StopsService(IStopsRepository stopsRepository, ILogger<StopsService> logger)
        {
            _stopsRepository = stopsRepository;
            _logger = logger;
        }

        public int ReadStopsFromFile(string filePath)
        {
            var allStops = new List<Stop>();
            var allStopsCount = 0;

            using (var streamReader = new StreamReader(filePath))
            {
                var stopString = streamReader.ReadLine();

                while ((stopString = streamReader.ReadLine()) != null)
                {
                    var createdStop = MapToStop(stopString);
                    allStops.Add(createdStop);
                }
                _logger.LogInformation($"Read {allStops.Count} stops.");
                _logger.LogInformation("Sorting stops by StopId...");
                allStops.Sort((stop1, stop2) => stop1.Id.CompareTo(stop2.Id));
                _logger.LogInformation("Stops have been sorted!");

                _stopsRepository.CreateStops(allStops);
                allStopsCount = allStops.Count;
                allStops.Clear();
                _logger.LogInformation("Stops have been successfully saved!");
            }

            return allStopsCount;
        }

        private Stop MapToStop(string stopString)
        {
            var stopInfos = stopString.Split(';');
            var id = stopInfos[2];
            var longitude = stopInfos[0];
            var latitude = stopInfos[1];
            var typeKey = stopInfos[3];

            var mappedStop = new Stop
            {
                Id = int.Parse(id),
                Longitude = longitude,
                Latitude = latitude,
            };

            return mappedStop;
        }
    }
}