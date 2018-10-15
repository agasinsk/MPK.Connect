using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using System;
using System.Collections.Generic;

namespace MPK.Connect.Service.Builders
{
    public class StopBuilder : BaseEntityBuilder<Stop>
    {
        public override Stop Build(string dataString, IDictionary<string, int> mappings)
        {
            var stopInfos = dataString.Replace("\"", "").Split(',');
            var id = stopInfos[mappings["stop_id"]];
            var code = mappings.ContainsKey("stop_code") ? stopInfos[mappings["stop_code"]] : null;
            var name = stopInfos[mappings["stop_name"]];
            var description = mappings.ContainsKey("stop_desc") ? stopInfos[mappings["stop_desc"]] : null;
            var longitude = stopInfos[mappings["stop_lat"]];
            var latitude = stopInfos[mappings["stop_lon"]];
            var zoneId = mappings.ContainsKey("zone_id") ? stopInfos[mappings["zone_id"]] : null;
            var url = mappings.ContainsKey("stop_url") ? stopInfos[mappings["stop_url"]] : null;
            Enum.TryParse(mappings.ContainsKey("location_type") ? stopInfos[mappings["location_type"]] : string.Empty, out LocationTypes locationType);
            var parentStation = mappings.ContainsKey("parent_station") ? stopInfos[mappings["parent_station"]] : null;
            var timeZone = mappings.ContainsKey("stop_timezone") ? stopInfos[mappings["stop_timezone"]] : null;
            Enum.TryParse(mappings.ContainsKey("wheelchair_boarding") ? stopInfos[mappings["wheelchair_boarding"]] : string.Empty, out WheelchairBoardings wheelchairBoarding);

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