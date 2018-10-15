using MPK.Connect.Model.Enums;
using MPK.Connect.Model.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPK.Connect.Model
{
    public class Frequency : IdentifiableEntity<string>
    {
        [NotMapped]
        public override string Id => $"{TripId}:{HeadwaySecs}";

        [Key, ForeignKey(nameof(Trip))]
        [Required]
        public string TripId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public long HeadwaySecs { get; set; }

        public ExactTimes ExactTimes { get; set; }

        public Trip Trip { get; set; }
    }
}