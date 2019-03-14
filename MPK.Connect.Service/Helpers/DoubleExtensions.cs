using System;

namespace MPK.Connect.Service.Helpers
{
    public static class DoubleExtensions
    {
        public static bool AlmostEquals(this double x, double y, double precision = 0.0001)
        {
            return x.CompareTo(y, precision) == 0;
        }

        public static bool LessThan(this double x, double y, double precision = 0.0001)
        {
            return x.CompareTo(y, precision) < 0;
        }

        public static bool MoreThan(this double x, double y, double precision = 0.0001)
        {
            return x.CompareTo(y, precision) > 0;
        }

        private static int CompareTo(this double x, double y, double precision = 0.0001)
        {
            return Math.Abs(x - y).CompareTo(precision);
        }
    }
}