using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MPK.Connect.Model.Business.TravelPlan
{
    public class TravelPlan
    {
        public string Id { get; set; }

        [JsonConverter(typeof(DateConverter))]
        public DateTime StartTime { get; set; }

        [JsonConverter(typeof(DateConverter))]
        public DateTime EndTime { get; set; }

        public double Duration { get; set; }
        public IEnumerable<string> RouteIds { get; set; }
        public int Transfers { get; set; }
        public StopDto Source { get; set; }
        public StopDto Destination { get; set; }
        public IEnumerable<StopTimeInfo> Stops { get; set; }
    }
}