using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using MPK.Connect.Service.Helpers;
using System;

namespace MPK.Connect.Service.Builders
{
    public class RouteBuilder : BaseEntityBuilder<Route>
    {
        public override Route Build(string dataString)
        {
            var data = dataString.Replace("\"", "").ToEntityData();

            var id = data[_entityMappings["route_id"]];
            var agencyId = GetNullableInt(data[_entityMappings["agency_id"]]).Value;
            var shortName = data[_entityMappings["route_short_name"]];
            var longName = data[_entityMappings["route_long_name"]];
            var desc = data[_entityMappings["route_desc"]];
            Enum.TryParse(data[_entityMappings["route_type"]], out RouteTypes routeType);
            var url = data[_entityMappings["route_url"]];
            var color = data[_entityMappings["route_color"]];
            var textColor = data[_entityMappings["route_text_color"]];
            var sortOrder = data[_entityMappings["route_sort_order"]];

            var mappedRoute = new Route
            {
                Id = id,
                AgencyId = agencyId,
                ShortName = shortName,
                LongName = longName,
                Description = desc,
                Type = routeType,
                Url = url,
                Color = color,
                TextColor = textColor,
                SortOrder = sortOrder
            };

            return mappedRoute;
        }
    }
}