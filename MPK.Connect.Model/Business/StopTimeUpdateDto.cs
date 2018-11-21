using System;

namespace MPK.Connect.Model.Business
{
    public class StopTimeUpdateDto : StopTimeDto
    {
        public TimeSpan UpdatedDepartureTime { get; set; }
    }
}