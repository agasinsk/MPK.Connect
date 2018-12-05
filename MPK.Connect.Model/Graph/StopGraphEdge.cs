using System;

namespace MPK.Connect.Model.Graph
{
    public class StopGraphEdge
    {
        public string StopId { get; set; }
        public string TripId { get; set; }
        public TimeSpan Cost { get; set; }
    }
}