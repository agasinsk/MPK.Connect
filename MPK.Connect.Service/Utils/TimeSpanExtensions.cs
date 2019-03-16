using System;

namespace MPK.Connect.Service.Utils
{
    public static class TimeSpanExtensions
    {
        public static DateTime ToDateTime(this TimeSpan time)
        {
            return time.ToDateTime(DateTime.Now);
        }

        public static DateTime ToDateTime(this TimeSpan time, DateTime date)
        {
            var newDate = date.Date + time;
            return newDate;
        }
    }
}