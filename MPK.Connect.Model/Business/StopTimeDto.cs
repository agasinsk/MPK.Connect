using System;

namespace MPK.Connect.Model.Business
{
    public class StopTimeDto
    {
        public TimeSpan ArrivalTime { get; set; }

        public TimeSpan DepartureTime { get; set; }

        public int StopSequence { get; set; }
    }
}