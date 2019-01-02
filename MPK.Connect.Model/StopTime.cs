using MPK.Connect.Model.Enums;
using MPK.Connect.Model.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPK.Connect.Model
{
    public class StopTime : IdentifiableEntity<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public override int Id { get; set; }

        [ForeignKey(nameof(Trip))]
        [Required]
        public int TripId { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan ArrivalTime { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan DepartureTime { get; set; }

        [Required]
        public int StopId { get; set; }

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