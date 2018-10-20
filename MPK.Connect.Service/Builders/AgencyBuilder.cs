using MPK.Connect.Model;
using MPK.Connect.Service.Helpers;

namespace MPK.Connect.Service.Builders
{
    public class AgencyBuilder : BaseEntityBuilder<Agency>
    {
        public override Agency Build(string dataString)
        {
            var data = dataString.Replace("\"", string.Empty).ToEntityData();

            var id = data[_entityMappings["agency_id"]];
            var name = data[_entityMappings["agency_name"]];
            var url = data[_entityMappings["agency_url"]];
            var timeZone = data[_entityMappings["agency_timezone"]];
            var language = data[_entityMappings["agency_lang"]];
            var phone = data[_entityMappings["agency_phone"]];
            var fareUrl = data[_entityMappings["agency_fare_url"]];
            var email = data[_entityMappings["agency_email"]];

            var agency = new Agency
            {
                Id = id,
                Name = name,
                Url = url,
                Phone = phone,
                Timezone = timeZone,
                Language = language,
                FareUrl = fareUrl,
                Email = email
            };
            return agency;
        }
    }
}