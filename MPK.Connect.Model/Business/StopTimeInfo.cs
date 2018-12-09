using System;

namespace MPK.Connect.Model.Business
{
    public class StopTimeInfo : LocalizableEntity<string>
    {
        public override string Id => $"{StopId}|{TripId}|{DepartureTime}";
        public string TripId { get; set; }
        public string Route { get; set; }
        public string StopId { get; set; }
        public int StopSequence { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan DepartureTime { get; set; }

        public StopDto Stop { get; set; }

        public override double GetDistanceTo(LocalizableEntity<string> otherEntity)
        {
            if (otherEntity is StopTimeInfo)
            {
                var stopTimeInfo = otherEntity as StopTimeInfo;
                return Stop.GetDistanceTo(stopTimeInfo.Stop);
            }

            return 0;
        }

        public override string ToString()
        {
            return $"{Stop.Name}, {Route}, {DepartureTime}";
        }
    }
}