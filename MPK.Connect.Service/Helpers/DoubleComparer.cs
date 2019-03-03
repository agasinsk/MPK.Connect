using System;
using System.Collections;
using System.Collections.Generic;

namespace MPK.Connect.Service.Helpers
{
    public static class DoubleExtensions
    {
        public static bool Equals(this double x, double y, double precision = 0.0001)
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
            var xAsInt = (int)(x / precision);
            var yAsInt = (int)(y / precision);

            return xAsInt.CompareTo(yAsInt);
        }
    }

    public class DoubleComparer : IComparer<double>, IComparer
    {
        private readonly double _epsilon;

        public DoubleComparer(double epsilon = 0.0001)
        {
            _epsilon = epsilon;
        }

        public int Compare(double x, double y)
        {
            return Math.Abs(x - y).CompareTo(_epsilon);
        }

        public int Compare(object x, object y)
        {
            if (!(x is double) || !(y is double))
            {
                return -1;
            }

            return Compare((double)x, (double)y);
        }
    }
}