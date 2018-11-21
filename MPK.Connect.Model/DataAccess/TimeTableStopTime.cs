using System;
using MPK.Connect.Model.Enums;

namespace MPK.Connect.Model.DataAccess
{
    public class TimeTableStopTime
    {
        public TimeSpan DepartureTime { get; set; }
        public string RouteId { get; set; }
        public string TripId { get; set; }
        public string Direction { get; set; }
        public RouteTypes RouteType { get; set; }
    }
}