using MPK.Connect.Model;
using System;
using System.Collections.Generic;

namespace MPK.Connect.Service.Builders
{
    public class FeedInfoBuilder : BaseEntityBuilder<FeedInfo>
    {
        public override FeedInfo Build(string dataString, IDictionary<string, int> mappings)
        {
            var data = dataString.Replace("\"", "").Split(',');

            var feedPublisherName = data[mappings["feed_publisher_name"]];
            var feedPublisherUrl = data[mappings["feed_publisher_url"]];
            var feedLang = data[mappings["feed_lang"]];

            var feedStartDate = mappings.ContainsKey("feed_start_date") ? DateTime.Parse(data[mappings["feed_start_date"]]) : (DateTime?)null;
            var feedEndDate = mappings.ContainsKey("feed_end_date") ? DateTime.Parse(data[mappings["feed_end_date"]]) : (DateTime?)null;

            var feedVersion = mappings.ContainsKey("feed_version") ? int.Parse(data[mappings["feed_version"]]) : (int?)null;

            var feedInfo = new FeedInfo
            {
                PublisherName = feedPublisherName,
                PublisherUrl = feedPublisherUrl,
                Language = feedLang,
                StartDate = feedStartDate,
                EndDate = feedEndDate,
                Version = feedVersion
            };
            return feedInfo;
        }
    }
}