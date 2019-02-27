using MPK.Connect.Model.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPK.Connect.Model
{
    public class FeedInfo : Identifiable<string>
    {
        [NotMapped]
        public override string Id => PublisherName;

        [Key]
        [Required]
        public string PublisherName { get; set; }

        [Required]
        public string PublisherUrl { get; set; }

        [Required]
        public string Language { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Version { get; set; }
    }
}