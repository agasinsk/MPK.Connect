using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using System;

namespace MPK.Connect.Service.Builders
{
    public class StopBuilder : BaseEntityBuilder<Stop>
    {
        public override Stop Build(string dataString)
        {
            var stopInfos = dataString.Replace("\"", "").Split(',');
            var id = stopInfos[_entityMappings["stop_id"]];
            var code = _entityMappings.ContainsKey("stop_code") ? stopInfos[_entityMappings["stop_code"]] : null;
            var name = stopInfos[_entityMappings["stop_name"]];
            var description = _entityMappings.ContainsKey("stop_desc") ? stopInfos[_entityMappings["stop_desc"]] : null;
            var longitude = stopInfos[_entityMappings["stop_lat"]];
            var latitude = stopInfos[_entityMappings["stop_lon"]];
            var zoneId = _entityMappings.ContainsKey("zone_id") ? stopInfos[_entityMappings["zone_id"]] : null;
            var url = _entityMappings.ContainsKey("stop_url") ? stopInfos[_entityMappings["stop_url"]] : null;
            Enum.TryParse(_entityMappings.ContainsKey("location_type") ? stopInfos[_entityMappings["location_type"]] : string.Empty, out LocationTypes locationType);
            var parentStation = _entityMappings.ContainsKey("parent_station") ? stopInfos[_entityMappings["parent_station"]] : null;
            var timeZone = _entityMappings.ContainsKey("stop_timezone") ? stopInfos[_entityMappings["stop_timezone"]] : null;
            Enum.TryParse(_entityMappings.ContainsKey("wheelchair_boarding") ? stopInfos[_entityMappings["wheelchair_boarding"]] : string.Empty, out WheelchairBoardings wheelchairBoarding);

            var mappedStop = new Stop
            {
                Id = id,
                Code = code,
                Name = name,
                Description = description,
                Longitude = double.Parse(longitude),
                Latitude = double.Parse(latitude),
                ZoneId = zoneId,
                Url = url,
                LocationType = locationType,
                ParentStation = parentStation,
                Timezone = timeZone,
                WheelchairBoarding = wheelchairBoarding
            };

            return mappedStop;
        }
    }
}