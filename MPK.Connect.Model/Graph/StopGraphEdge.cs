using System;

namespace MPK.Connect.Model.Graph
{
    public class StopGraphEdge
    {
        public string StopId { get; set; }
        public string TripId { get; set; }
        public int Cost { get; set; }
        public TimeSpan DepartureTime { get; set; }
    }

    public class TravelCost
    {
        public int Cost { get; set; }

        public TimeSpan DepartureTime { get; set; }

        public TravelCost(int cost, TimeSpan departureTime)
        {
            Cost = cost;
            DepartureTime = departureTime;
        }

        public TravelCost()
        {
        }

        public override string ToString()
        {
            return $"{Cost}, {DepartureTime.Hours}:{DepartureTime.Minutes}";
        }
    }
}