using MPK.Connect.Model;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess.Stops;

namespace MPK.Connect.Service
{
    public class StopService : GenericService<Stop>
    {
        public StopService(IStopRepository stopsRepository, ILogger<StopService> logger) : base(stopsRepository, logger)
        {
        }

        protected override Stop Map(string entityString)
        {
            var stopInfos = entityString.Split(',');
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

        protected override void SortEntities(List<Stop> entities)
        {
            entities.Sort((stop1, stop2) => stop1.StopId.CompareTo(stop2.Id));
        }
    }
}