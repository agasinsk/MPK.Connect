using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using MPK.Connect.Service.Helpers;
using System;

namespace MPK.Connect.Service.Builders
{
    public class TripBuilder : BaseEntityBuilder<Trip>
    {
        public override Trip Build(string dataString)
        {
            var data = dataString.Replace("\"", "").ToEntityData();

            var routeId = data[_entityMappings["route_id"]];
            var serviceId = GetNullableInt(data[_entityMappings["service_id"]]).GetValueOrDefault();
            var tripId = GetNullableInt(data[_entityMappings["trip_id"]]).Value;
            var headSign = data[_entityMappings["trip_headsign"]];
            var shortName = data[_entityMappings["trip_short_name"]];
            var directionId = GetNullableInt(data[_entityMappings["direction_id"]]);
            var blockId = data[_entityMappings["block_id"]];
            var shapeId = data[_entityMappings["shape_id"]];
            Enum.TryParse(data[_entityMappings["wheelchair_accessible"]], out WheelchairBoardings wheelchair);
            Enum.TryParse(data[_entityMappings["bikes_allowed"]], out BikesAllowed bikesAllowed);

            var mappedStop = new Trip
            {
                Id = tripId,
                RouteId = routeId,
                ServiceId = serviceId,
                HeadSign = headSign,
                ShortName = shortName,
                DirectionId = directionId,
                BlockId = blockId,
                ShapeId = shapeId,
                WheelchairAccessible = wheelchair,
                BikesAllowed = bikesAllowed
            };

            return mappedStop;
        }
    }
}