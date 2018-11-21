using System;

namespace MPK.Connect.Model.Business
{
    public class StopTimeCore
    {
        public string TripId { get; set; }
        public TimeSpan DepartureTime { get; set; }
    }
}