using MPK.Connect.Model;
using System.Collections.Generic;

namespace MPK.Connect.Service.Builders
{
    public class StopBuilder : IEntityBuilder<Stop>
    {
        public Stop Build(string dataString, IDictionary<string, int> mappings)
        {
            var stopInfos = dataString.Replace("\"", "").Split(',');
            var id = stopInfos[0];
            var code = stopInfos[1];
            var name = stopInfos[2];
            var description = stopInfos[3];
            var longitude = stopInfos[4];
            var latitude = stopInfos[5];

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

        public IDictionary<string, int> GetEntityMappings(string headerString)
        {
            throw new System.NotImplementedException();
        }
    }
}