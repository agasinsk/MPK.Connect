using MPK.Connect.Model;
using System.Collections.Generic;

namespace MPK.Connect.Service.Builders
{
    public class AgencyBuilder : BaseEntityBuilder<Agency>
    {
        public override Agency Build(string dataString, IDictionary<string, int> mappings)
        {
            var agencyData = dataString.Replace("\"", string.Empty).Split(',');
            var id = mappings.ContainsKey("agency_id") ? agencyData[mappings["agency_id"]] : null;
            var name = agencyData[mappings["agency_name"]];
            var url = agencyData[mappings["agency_url"]];
            var timeZone = agencyData[mappings["agency_timezone"]];
            var language = mappings.ContainsKey("agency_lang") ? agencyData[mappings["agency_lang"]] : null;
            var phone = mappings.ContainsKey("agency_phone") ? agencyData[mappings["agency_phone"]] : null;
            var fareUrl = mappings.ContainsKey("agency_fare_url") ? agencyData[mappings["agency_fare_url"]] : null;
            var email = mappings.ContainsKey("agency_email") ? agencyData[mappings["agency_email"]] : null;

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