using MPK.Connect.Model.Enums;
using MPK.Connect.Model.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPK.Connect.Model
{
    public class CalendarDate : Identifiable<string>
    {
        [NotMapped]
        public override string Id => ServiceId;

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