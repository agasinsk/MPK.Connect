using MPK.Connect.Model;
using MPK.Connect.Service.Helpers;

namespace MPK.Connect.Service.Builders
{
    public class FeedInfoBuilder : BaseEntityBuilder<FeedInfo>
    {
        public override FeedInfo Build(string dataString)
        {
            var data = dataString.Replace("\"", "").ToEntityData();

            var feedPublisherName = data[_entityMappings["feed_publisher_name"]];
            var feedPublisherUrl = data[_entityMappings["feed_publisher_url"]];
            var feedLang = data[_entityMappings["feed_lang"]];

            var feedStartDate = GetDate(data[_entityMappings["feed_start_date"]]);
            var feedEndDate = GetDate(data[_entityMappings["feed_end_date"]]);
            var feedVersion = GetNullableInt(data[_entityMappings["feed_version"]]);

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