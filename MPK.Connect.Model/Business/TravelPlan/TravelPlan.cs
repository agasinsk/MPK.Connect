using System;
using System.Collections.Generic;

namespace MPK.Connect.Model.Business.TravelPlan
{
    public class TravelPlan
    {
        public string Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Location Source { get; set; }
        public Location Destination { get; set; }
        public IEnumerable<StopTimeInfo> Stops { get; set; }
    }
}