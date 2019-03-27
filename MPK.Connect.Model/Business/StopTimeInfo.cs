using System;
using MPK.Connect.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MPK.Connect.Model.Business
{
    public class StopTimeInfo : LocalizableEntity<int>
    {
        [JsonConverter(typeof(DailyTimeSpanConverter))]
        public TimeSpan ArrivalTime { get; set; }

        [JsonConverter(typeof(DailyTimeSpanConverter))]
        public TimeSpan DepartureTime { get; set; }

        public string Direction { get; set; }

        [JsonIgnore]
        public int? DirectionId { get; set; }

        [JsonIgnore]
        public override int Id { get; set; }

        public string Route { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RouteTypes RouteType { get; set; }

        [JsonProperty("stopInfo")]
        public StopDto StopDto { get; set; }

        [JsonIgnore]
        public int StopId { get; set; }

        public int StopSequence { get; set; }

        public int TripId { get; set; }

        public override double GetDistanceTo(LocalizableEntity<int> otherEntity)
        {
            if (otherEntity is StopTimeInfo stopTimeInfo)
            {
                return StopDto.GetDistanceTo(stopTimeInfo.StopDto);
            }

            return double.MaxValue;
        }

        public override double GetDistanceTo(double latitude, double longitude)
        {
            return StopDto.GetDistanceTo(latitude, longitude);
        }

        public override string ToString()
        {
            return $"{StopDto.Name}, {Route}, {DepartureTime}";
        }
    }
}