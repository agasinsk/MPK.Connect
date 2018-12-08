using System;

namespace MPK.Connect.Model.Graph
{
    public class StopTimeGraphEdge
    {
        public string StopId { get; set; }
        public int Cost { get; set; }
        public TimeSpan DepartureTime { get; set; }
    }
}