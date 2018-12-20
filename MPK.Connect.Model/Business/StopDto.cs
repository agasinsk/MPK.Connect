using System;
using MPK.Connect.Model.Helpers;

namespace MPK.Connect.Model.Business
{
    public class StopDto : IdentifiableEntity<string>
    {
        public string Code { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; }

        public double GetDistanceTo(StopDto destination)
        {
            if (destination == null || destination == this)
            {
                return double.MaxValue;
            }

            return GetDistanceTo(destination.Latitude, destination.Longitude);
        }

        public double GetDistanceTo(double destinationLatitude, double destinationLongitude)
        {
            const double earthRadius = 6371; //km

            var φ1 = Latitude.ToRadians();
            var φ2 = destinationLatitude.ToRadians();
            var Δφ = (destinationLatitude - Latitude).ToRadians();
            var Δλ = (destinationLongitude - Longitude).ToRadians();

            var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                    Math.Cos(φ1) * Math.Cos(φ2) *
                    Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = earthRadius * c;
            return distance;
        }
    }
}