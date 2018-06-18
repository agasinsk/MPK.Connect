using System;
using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    public class CalendarDate
    {
        public DateTime Date { get; set; }
        public ExceptionRules ExceptionRule { get; set; }

        [Key]
        public int ServiceId { get; set; }
    }
}