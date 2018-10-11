using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess.Stops;
using MPK.Connect.Model;

namespace MPK.Connect.Service
{
    public class StopService : ImporterService<Stop>
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
                Id = id,
                Code = code,
                Name = name,
                Longitude = double.Parse(longitude),
                Latitude = double.Parse(latitude),
            };

            return mappedStop;
        }
    }
}