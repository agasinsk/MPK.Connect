using System;
using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    public class Calendar
    {
        public DateTime EndDate { get; set; }
        public bool Friday { get; set; }
        public bool Monday { get; set; }
        public bool Saturday { get; set; }

        [Key]
        public int ServiceId { get; set; }

        public DateTime StartDate { get; set; }
        public bool Sunday { get; set; }
        public bool Thursday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
    }
}