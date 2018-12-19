using System;

namespace MPK.Connect.Service.Helpers
{
    public static class TimeSpanExtensions
    {
        public static DateTime ToDateTime(this TimeSpan time)
        {
            var now = DateTime.Now;
            return new DateTime(now.Year, now.Month, now.Day, time.Hours, time.Minutes, time.Seconds);
        }
    }
}