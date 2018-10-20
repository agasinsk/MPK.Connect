using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using System;

namespace MPK.Connect.Service.Builders
{
    public class RouteBuilder : BaseEntityBuilder<Route>
    {
        public override Route Build(string dataString)
        {
            var routeData = dataString.Replace("\"", "").Split(',');
            var id = routeData[_entityMappings["route_id"]];
            var agencyId = _entityMappings.ContainsKey("agency_id") ? routeData[_entityMappings["agency_id"]] : null;
            var shortName = routeData[_entityMappings["route_short_name"]];
            var longName = routeData[_entityMappings["route_long_name"]];
            var desc = _entityMappings.ContainsKey("route_desc") ? routeData[_entityMappings["route_desc"]] : null;
            Enum.TryParse(_entityMappings.ContainsKey("route_type") ? routeData[_entityMappings["route_type"]] : string.Empty, out RouteTypes routeType);
            var url = _entityMappings.ContainsKey("route_url") ? routeData[_entityMappings["route_url"]] : null;
            var color = _entityMappings.ContainsKey("route_color") ? routeData[_entityMappings["route_color"]] : null;
            var textColor = _entityMappings.ContainsKey("route_text_color") ? routeData[_entityMappings["route_text_color"]] : null;
            var sortOrder = _entityMappings.ContainsKey("route_sort_order") ? routeData[_entityMappings["route_sort_order"]] : null;

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