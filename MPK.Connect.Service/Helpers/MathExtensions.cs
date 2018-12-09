using System;
using MPK.Connect.Model;

namespace MPK.Connect.Service.Helpers
{
    public static class MathExtensions
    {
        public static double ToRadians(this double angle)
        {
            return angle * 2 * Math.PI / 360;
        }

        public static double GetDistanceTo(this Stop source, Stop destination)
        {
            if (destination == null || destination == source)
            {
                return 0d;
            }

            const double earthRadius = 6371; //km

            var φ1 = source.Latitude.ToRadians();
            var φ2 = destination.Latitude.ToRadians();
            var Δφ = (destination.Latitude - source.Latitude).ToRadians();
            var Δλ = (destination.Longitude - source.Longitude).ToRadians();

            var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                    Math.Cos(φ1) * Math.Cos(φ2) *
                    Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = earthRadius * c;
            return distance;
        }
    }
}