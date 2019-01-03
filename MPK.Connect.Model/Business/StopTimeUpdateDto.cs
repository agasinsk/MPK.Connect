using System;

namespace MPK.Connect.Model.Business
{
    public class StopTimeUpdateDto
    {
        public int Id { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan UpdatedDepartureTime { get; set; }
    }
}