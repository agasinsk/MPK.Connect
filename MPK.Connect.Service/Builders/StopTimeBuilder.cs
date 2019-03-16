﻿using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using MPK.Connect.Service.Helpers;
using System;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Builders
{
    public class StopTimeBuilder : BaseEntityBuilder<StopTime>
    {
        public override StopTime Build(string dataString)
        {
            var data = dataString.Replace("\"", "").ToEntityData();

            var tripId = GetNullableInt(data[_entityMappings["trip_id"]]).Value;
            var arrival = GetTime(data[_entityMappings["arrival_time"]]).GetValueOrDefault();
            var departure = GetTime(data[_entityMappings["departure_time"]]).GetValueOrDefault();
            var stopId = GetNullableInt(data[_entityMappings["stop_id"]]).Value;
            var stopSequence = GetNullableInt(data[_entityMappings["stop_sequence"]]).GetValueOrDefault();
            var stopHeadsign = data[_entityMappings["stop_headsign"]];

            Enum.TryParse(data[_entityMappings["pickup_type"]], out PickupTypes pickup);
            Enum.TryParse(data[_entityMappings["drop_off_type"]], out DropOffTypes dropOff);
            var distTraveled = GetDouble(data[_entityMappings["shape_dist_traveled"]]);

            var timePoints = Enum.TryParse(data[_entityMappings["timepoint"]], out TimePoints timePoint) ? timePoint : TimePoints.Exact;

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
                ShapeDistTraveled = distTraveled,
                TimePoint = timePoints
            };

            return mappedStop;
        }
    }
}