using MPK.Connect.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPK.Connect.Model
{
    public class CalendarDate
    {
        [Key]
        public string ServiceId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public ExceptionRules ExceptionRule { get; set; }

        [ForeignKey("ServiceId")]
        public Calendar Calendar { get; set; }
    }
}