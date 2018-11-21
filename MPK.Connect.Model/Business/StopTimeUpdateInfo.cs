using System;

namespace MPK.Connect.Model.Business
{
    public class StopTimeUpdateInfo : StopTimeInfo
    {
        public TimeSpan UpdatedDepartureTime { get; set; }
    }
}