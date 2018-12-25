using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MPK.Connect.Model.Enums;
using MPK.Connect.Model.Helpers;

namespace MPK.Connect.Model
{
    public class StopTime : IdentifiableEntity<string>
    {
        [ForeignKey(nameof(Trip))]
        [Required]
        public string TripId { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan ArrivalTime { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan DepartureTime { get; set; }

        [Required]
        public string StopId { get; set; }

        [Required]
        public int StopSequence { get; set; }

        public string HeadSign { get; set; }

        public PickupTypes PickupType { get; set; }

        public DropOffTypes DropOffTypes { get; set; }

        public double? ShapeDistTraveled { get; set; }

        public TimePoints TimePoint { get; set; }

        public Stop Stop { get; set; }

        public Trip Trip { get; set; }
    }
}