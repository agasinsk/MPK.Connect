using System;

namespace MPK.Connect.Model
{
    public class FeedInfo
    {
        public DateTime EndDate { get; set; }
        public int Id { get; set; }
        public string Language { get; set; }
        public string PublisherName { get; set; }
        public string PublisherUrl { get; set; }
        public DateTime StartDate { get; set; }
    }
}