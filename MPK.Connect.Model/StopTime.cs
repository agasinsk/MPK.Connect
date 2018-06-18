using System;

namespace MPK.Connect.Model
{
    public class StopTime
    {
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public DropOffType DropOffType { get; set; }
        public int Id { get; set; }
        public PickupTypes PickupType { get; set; }
        public Stop Stop { get; set; }
        public int StopId { get; set; }
        public int StopSequence { get; set; }
        public Trip Trip { get; set; }
        public int TripId { get; set; }
    }
}