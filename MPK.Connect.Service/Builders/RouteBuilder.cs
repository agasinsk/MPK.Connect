using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using System;
using System.Collections.Generic;

namespace MPK.Connect.Service.Builders
{
    public class RouteBuilder : BaseEntityBuilder<Route>
    {
        public override Route Build(string dataString, IDictionary<string, int> mappings)
        {
            var routeData = dataString.Replace("\"", "").Split(',');
            var id = routeData[mappings["route_id"]];
            var agencyId = mappings.ContainsKey("agency_id") ? routeData[mappings["agency_id"]] : null;
            var shortName = routeData[mappings["route_short_name"]];
            var longName = routeData[mappings["route_long_name"]];
            var desc = mappings.ContainsKey("route_desc") ? routeData[mappings["route_desc"]] : null;
            Enum.TryParse(mappings.ContainsKey("route_type") ? routeData[mappings["route_type"]] : string.Empty, out RouteTypes routeType);
            var url = mappings.ContainsKey("route_url") ? routeData[mappings["route_url"]] : null;
            var color = mappings.ContainsKey("route_color") ? routeData[mappings["route_color"]] : null;
            var textColor = mappings.ContainsKey("route_text_color") ? routeData[mappings["route_text_color"]] : null;
            var sortOrder = mappings.ContainsKey("route_sort_order") ? routeData[mappings["route_sort_order"]] : null;

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