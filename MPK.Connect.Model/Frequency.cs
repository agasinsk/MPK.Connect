using MPK.Connect.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPK.Connect.Model
{
    public class Frequency
    {
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