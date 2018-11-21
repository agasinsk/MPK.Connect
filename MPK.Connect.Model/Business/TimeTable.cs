using System.Collections.Generic;

namespace MPK.Connect.Model.Business
{
    public class TimeTable
    {
        public string StopId { get; set; }
        public string StopCode { get; set; }
        public string StopName { get; set; }

        public Dictionary<string, RouteStopTimes> RouteTimes { get; set; }
    }
}