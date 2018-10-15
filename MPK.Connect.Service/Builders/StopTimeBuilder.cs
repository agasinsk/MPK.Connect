using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using System;
using System.Collections.Generic;

namespace MPK.Connect.Service.Builders
{
    public class StopTimeBuilder : BaseEntityBuilder<StopTime>
    {
        public override StopTime Build(string dataString, IDictionary<string, int> mappings)
        {
            var stopInfos = dataString.Replace("\"", "").Split(',');
            var tripId = stopInfos[mappings["trip_id"]];
            var arrival = DateTime.Parse(stopInfos[mappings["arrival_time"]]);
            var departure = DateTime.Parse(stopInfos[mappings["departure_time"]]);
            var stopId = stopInfos[mappings["stop_id"]];
            var stopSequence = int.Parse(stopInfos[mappings["stop_sequence"]]);
            var stopHeadsign = mappings.ContainsKey("stop_headsign") ? stopInfos[mappings["stop_headsign"]] : null;

            Enum.TryParse(mappings.ContainsKey("pickup_type") ? stopInfos[mappings["pickup_type"]] : string.Empty, out PickupTypes pickup);
            Enum.TryParse(mappings.ContainsKey("drop_off_type") ? stopInfos[mappings["drop_off_type"]] : string.Empty, out DropOffTypes dropOff);
            var distTraveled = mappings.ContainsKey("shape_dist_traveled") ? stopInfos[mappings["shape_dist_traveled"]] : null;

            Enum.TryParse(mappings.ContainsKey("timepoint") ? stopInfos[mappings["timepoint"]] : string.Empty, out TimePoints timePoint);

            var mappedStop = new StopTime
            {
                TripId = tripId,
                ArrivalTime = arrival,
                DepartureTime = departure,
                StopId = stopId,
                StopSequence = stopSequence,
                HeadSign = stopHeadsign,
                PickupType = pickup,
                DropOffTypes = dropOff,
                ShapeDistTraveled = double.Parse(distTraveled),
                TimePoint = timePoint
            };

            return mappedStop;
        }
    }
}