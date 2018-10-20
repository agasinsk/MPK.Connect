using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using System;

namespace MPK.Connect.Service.Builders
{
    public class StopTimeBuilder : BaseEntityBuilder<StopTime>
    {
        public override StopTime Build(string dataString)
        {
            var stopInfos = dataString.Replace("\"", "").Split(',');
            var tripId = stopInfos[_entityMappings["trip_id"]];
            var arrival = GetDateTime(stopInfos[_entityMappings["arrival_time"]]).GetValueOrDefault();
            var departure = GetDateTime(stopInfos[_entityMappings["departure_time"]]).GetValueOrDefault();
            var stopId = stopInfos[_entityMappings["stop_id"]];
            var stopSequence = int.Parse(stopInfos[_entityMappings["stop_sequence"]]);
            var stopHeadsign = _entityMappings.ContainsKey("stop_headsign") ? stopInfos[_entityMappings["stop_headsign"]] : null;

            Enum.TryParse(_entityMappings.ContainsKey("pickup_type") ? stopInfos[_entityMappings["pickup_type"]] : string.Empty, out PickupTypes pickup);
            Enum.TryParse(_entityMappings.ContainsKey("drop_off_type") ? stopInfos[_entityMappings["drop_off_type"]] : string.Empty, out DropOffTypes dropOff);
            var distTraveled = _entityMappings.ContainsKey("shape_dist_traveled") ? stopInfos[_entityMappings["shape_dist_traveled"]] : null;

            Enum.TryParse(_entityMappings.ContainsKey("timepoint") ? stopInfos[_entityMappings["timepoint"]] : string.Empty, out TimePoints timePoint);

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