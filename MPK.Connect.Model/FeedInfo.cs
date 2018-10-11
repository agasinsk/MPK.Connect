using System;
using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    public class FeedInfo
    {
        [Key]
        [Required]
        public string PublisherName { get; set; }

        [Required]
        public string PublisherUrl { get; set; }

        [Required]
        public string Language { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Version { get; set; }
    }
}