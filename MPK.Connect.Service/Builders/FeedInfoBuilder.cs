using MPK.Connect.Model;

namespace MPK.Connect.Service.Builders
{
    public class FeedInfoBuilder : BaseEntityBuilder<FeedInfo>
    {
        public override FeedInfo Build(string dataString)
        {
            var data = dataString.Replace("\"", "").Split(',');

            var feedPublisherName = data[_entityMappings["feed_publisher_name"]];
            var feedPublisherUrl = data[_entityMappings["feed_publisher_url"]];
            var feedLang = data[_entityMappings["feed_lang"]];

            var feedStartDate = _entityMappings.ContainsKey("feed_end_date") ? GetDateTime(data[_entityMappings["feed_start_date"]]) : null;
            var feedEndDate = _entityMappings.ContainsKey("feed_end_date") ? GetDateTime(data[_entityMappings["feed_end_date"]]) : null;
            var feedVersion = _entityMappings.ContainsKey("feed_version") ? int.Parse(data[_entityMappings["feed_version"]]) : (int?)null;

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