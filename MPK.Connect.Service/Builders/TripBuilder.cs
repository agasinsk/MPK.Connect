using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using System;
using System.Collections.Generic;

namespace MPK.Connect.Service.Builders
{
    public class TripBuilder : BaseEntityBuilder<Trip>
    {
        public override Trip Build(string dataString, IDictionary<string, int> mappings)
        {
            var tripData = dataString.Replace("\"", "").Split(',');
            var routeId = tripData[mappings["route_id"]];
            var serviceId = tripData[mappings["service_id"]];
            var tripId = tripData[mappings["trip_id"]];
            var headSign = mappings.ContainsKey("trip_headsign") ? tripData[mappings["trip_headsign"]] : null;
            var shortName = mappings.ContainsKey("trip_short_name") ? tripData[mappings["trip_short_name"]] : null;
            var directionId = mappings.ContainsKey("direction_id") ? int.Parse(tripData[mappings["direction_id"]]) : (int?)null;
            var blockId = mappings.ContainsKey("block_id") ? tripData[mappings["block_id"]] : null;
            var shapeId = mappings.ContainsKey("shape_id") ? tripData[mappings["shape_id"]] : null;
            Enum.TryParse(mappings.ContainsKey("wheelchair_accessible") ? tripData[mappings["wheelchair_accessible"]] : string.Empty, out WheelchairBoardings wheelchair);
            Enum.TryParse(mappings.ContainsKey("bikes_allowed") ? tripData[mappings["bikes_allowed"]] : string.Empty, out BikesAllowed bikesAllowed);

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