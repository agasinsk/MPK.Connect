using System;

namespace MPK.Connect.Model.Helpers
{
    public static class MathExtensions
    {
        public static double ToRadians(this double angle)
        {
            return angle * 2 * Math.PI / 360;
        }
    }
}