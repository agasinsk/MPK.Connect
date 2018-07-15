using MPK.Connect.Model;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess.Stops;

namespace MPK.Connect.Service.Service.Stops
{
    public class StopService : IGenericService<Stop>
    {
        private readonly ILogger<StopService> _logger;
        private readonly IStopRepository _stopsRepository;

        public StopService(IStopRepository stopsRepository, ILogger<StopService> logger)
        {
            _stopsRepository = stopsRepository;
            _logger = logger;
        }

        public int ReadFromFile(string filePath)
        {
            var allStops = new List<Stop>();
            int allStopsCount;

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
                allStops.Sort((stop1, stop2) => stop1.StopId.CompareTo(stop2.Id));
                _logger.LogInformation("Stops have been sorted!");

                _stopsRepository.AddRange(allStops);
                _stopsRepository.Save();
                allStopsCount = allStops.Count;
                allStops.Clear();
                _logger.LogInformation("Stops have been successfully saved!");
            }

            return allStopsCount;
        }

        private Stop MapToStop(string stopString)
        {
            var stopInfos = stopString.Split(',');
            var id = stopInfos[0];
            var code = stopInfos[1];
            var name = stopInfos[2];
            var longitude = stopInfos[3];
            var latitude = stopInfos[4];

            var mappedStop = new Stop
            {
                StopId = int.Parse(id),
                Code = code,
                Name = name,
                Longitude = double.Parse(longitude),
                Latitude = double.Parse(latitude),
            };

            return mappedStop;
        }
    }
}