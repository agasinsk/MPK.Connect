using System.Collections.Generic;
using MPK.Connect.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MPK.Connect.Model.Business
{
    public class RouteStopTimes
    {
        public string RouteId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RouteTypes RouteType { get; set; }

        public List<DirectionStopTimes> Directions { get; set; }
    }
}