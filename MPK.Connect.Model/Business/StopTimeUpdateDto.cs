using System;

namespace MPK.Connect.Model.Business
{
    public class StopTimeUpdateDto : StopTimeDto
    {
        public TimeSpan UpdatedDepartureTime { get; set; }

        public override string ToString()
        {
            return $"{nameof(TripId)}:{TripId}, {nameof(StopId)}:{StopId}, {nameof(DepartureTime)}:{DepartureTime}";
        }
    }
}