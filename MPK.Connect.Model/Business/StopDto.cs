using System;
using MPK.Connect.Model.Helpers;

namespace MPK.Connect.Model.Business
{
    public class StopDto : Identifiable<int>
    {
        public string Code { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; }

        public int GetDistanceTo(StopDto destination)
        {
            if (destination == null)
            {
                return int.MaxValue;
            }

            if (destination == this)
            {
                return 0;
            }

            return GetDistanceTo(destination.Latitude, destination.Longitude);
        }

        public int GetDistanceTo(double destinationLatitude, double destinationLongitude)
        {
            const int earthRadius = 6371000; //km

            var φ1 = Latitude.ToRadians();
            var φ2 = destinationLatitude.ToRadians();
            var Δφ = (destinationLatitude - Latitude).ToRadians();
            var Δλ = (destinationLongitude - Longitude).ToRadians();

            var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                    Math.Cos(φ1) * Math.Cos(φ2) *
                    Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = (int)(earthRadius * c);
            return distance;
        }

        public override string ToString()
        {
            return $"{Name}:{Code}";
        }
    }
}