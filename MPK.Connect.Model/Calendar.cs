using System;
using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    public class Calendar
    {
        [Key]
        public string ServiceId { get; set; }

        [Required]
        public bool Monday { get; set; }

        [Required]
        public bool Tuesday { get; set; }

        [Required]
        public bool Wednesday { get; set; }

        [Required]
        public bool Thursday { get; set; }

        [Required]
        public bool Friday { get; set; }

        [Required]
        public bool Saturday { get; set; }

        [Required]
        public bool Sunday { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}