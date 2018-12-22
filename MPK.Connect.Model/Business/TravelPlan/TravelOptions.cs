using System;

namespace MPK.Connect.Model.Business.TravelPlan
{
    public class TravelOptions
    {
        public Location Destination { get; set; }
        public Location Source { get; set; }
        public DateTime? StartDate { get; set; }
    }
}