using System;

namespace MPK.Connect.Model.Business
{
    public class StopTimeDto
    {
        public string TripId { get; set; }
        public string StopId { get; set; }
        public TimeSpan DepartureTime { get; set; }
    }
}