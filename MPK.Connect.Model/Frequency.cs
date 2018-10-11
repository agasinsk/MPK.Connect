using MPK.Connect.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    public class Frequency
    {
        [Required]
        public string TripId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public long HeadwaySecs { get; set; }

        public ExactTimes ExactTimes { get; set; }
    }
}