using System;
using System.Collections.Generic;

namespace MPK.Connect.Model.Business.TravelPlan
{
    public class TravelPlan
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Duration { get; set; }
        public IEnumerable<string> RouteIds { get; set; }
        public int Transfers { get; set; }
        public StopDto Source { get; set; }
        public StopDto Destination { get; set; }
        public IEnumerable<StopTimeInfo> Stops { get; set; }
    }
}