using System;
using System.Collections.Generic;
using MPK.Connect.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MPK.Connect.Model.Business
{
    public class TimeTable
    {
        public string StopId { get; set; }
        public string StopCode { get; set; }
        public string StopName { get; set; }

        public Dictionary<string, RouteStopTimes> RouteTimes { get; set; }
    }

    public class RouteStopTimes
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public RouteTypes RouteType { get; set; }

        public List<DirectionStopTimes> Directions { get; set; }
    }

    public class DirectionStopTimes
    {
        public string Direction { get; set; }
        public List<TimeSpan> StopTimes { get; set; }
    }
}