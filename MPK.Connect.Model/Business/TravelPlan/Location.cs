using System;

namespace MPK.Connect.Model.Business.TravelPlan
{
    public class Location
    {
        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public string Name { get; set; }

        public Location()
        {
        }

        public Location(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Location(string name, double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}