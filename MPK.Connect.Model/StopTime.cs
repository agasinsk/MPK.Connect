using MPK.Connect.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPK.Connect.Model
{
    public class StopTime
    {
        [Key, ForeignKey(nameof(Trip))]
        [Required]
        public string TripId { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public string StopId { get; set; }

        [Required]
        public int StopSequence { get; set; }

        public string HeadSign { get; set; }

        public PickupTypes PickupType { get; set; }

        public DropOffTypes DropOffTypes { get; set; }

        public double ShapeDistTraveled { get; set; }

        public TimePoints TimePoint { get; set; }

        public Stop Stop { get; set; }

        public Trip Trip { get; set; }
    }
}