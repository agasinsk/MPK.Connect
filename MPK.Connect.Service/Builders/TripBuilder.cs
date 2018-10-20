using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using System;

namespace MPK.Connect.Service.Builders
{
    public class TripBuilder : BaseEntityBuilder<Trip>
    {
        public override Trip Build(string dataString)
        {
            var data = dataString.Replace("\"", "").Split(',');

            var routeId = data[_entityMappings["route_id"]];
            var serviceId = data[_entityMappings["service_id"]];
            var tripId = data[_entityMappings["trip_id"]];
            var headSign = _entityMappings.ContainsKey("trip_headsign") ? data[_entityMappings["trip_headsign"]] : null;
            var shortName = _entityMappings.ContainsKey("trip_short_name") ? data[_entityMappings["trip_short_name"]] : null;
            var directionId = _entityMappings.ContainsKey("direction_id") ? int.Parse(data[_entityMappings["direction_id"]]) : (int?)null;
            var blockId = _entityMappings.ContainsKey("block_id") ? data[_entityMappings["block_id"]] : null;
            var shapeId = _entityMappings.ContainsKey("shape_id") ? data[_entityMappings["shape_id"]] : null;
            Enum.TryParse(_entityMappings.ContainsKey("wheelchair_accessible") ? data[_entityMappings["wheelchair_accessible"]] : string.Empty, out WheelchairBoardings wheelchair);
            Enum.TryParse(_entityMappings.ContainsKey("bikes_allowed") ? data[_entityMappings["bikes_allowed"]] : string.Empty, out BikesAllowed bikesAllowed);

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