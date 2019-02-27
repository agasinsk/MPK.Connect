using MPK.Connect.Model.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPK.Connect.Model
{
    public class Calendar : Identifiable<int>
    {
        [NotMapped]
        public override int Id => ServiceId;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int ServiceId { get; set; }

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