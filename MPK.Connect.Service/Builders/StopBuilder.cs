using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using MPK.Connect.Service.Helpers;
using System;

namespace MPK.Connect.Service.Builders
{
    public class StopBuilder : BaseEntityBuilder<Stop>
    {
        public override Stop Build(string dataString)
        {
            var data = dataString.Replace("\"", "").ToEntityData();

            var id = data[_entityMappings["stop_id"]];
            var code = data[_entityMappings["stop_code"]];
            var name = data[_entityMappings["stop_name"]];
            var description = data[_entityMappings["stop_desc"]];
            var longitude = GetDouble(data[_entityMappings["stop_lon"]]).Value;
            var latitude = GetDouble(data[_entityMappings["stop_lat"]]).Value;
            var zoneId = data[_entityMappings["zone_id"]];
            var url = data[_entityMappings["stop_url"]];
            Enum.TryParse(data[_entityMappings["location_type"]], out LocationTypes locationType);
            var parentStation = data[_entityMappings["parent_station"]];
            var timeZone = data[_entityMappings["stop_timezone"]];
            Enum.TryParse(data[_entityMappings["wheelchair_boarding"]], out WheelchairBoardings wheelchairBoarding);

            var mappedStop = new Stop
            {
                Id = id,
                Code = code,
                Name = name,
                Description = description,
                Longitude = longitude,
                Latitude = latitude,
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