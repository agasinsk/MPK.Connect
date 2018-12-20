using System;
using MPK.Connect.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MPK.Connect.Model.Business
{
    public class StopTimeInfo : LocalizableEntity<string>
    {
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public override string Id => $"{StopId}|{TripId}|{DepartureTime}";
        public string Route { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RouteTypes RouteType { get; set; }

        public StopDto Stop { get; set; }
        public string StopId { get; set; }
        public int StopSequence { get; set; }
        public string TripId { get; set; }

        public override double GetDistanceTo(LocalizableEntity<string> otherEntity)
        {
            if (otherEntity is StopTimeInfo stopTimeInfo)
            {
                return Stop.GetDistanceTo(stopTimeInfo.Stop);
            }

            return double.MaxValue;
        }

        public override string ToString()
        {
            return $"{Stop.Name}, {Route}, {DepartureTime}";
        }
    }
}