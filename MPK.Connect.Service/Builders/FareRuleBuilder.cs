using MPK.Connect.Model;
using System.Collections.Generic;

namespace MPK.Connect.Service.Builders
{
    public class FareRuleBuilder : BaseEntityBuilder<FareRule>
    {
        public override FareRule Build(string dataString, IDictionary<string, int> mappings)
        {
            var data = dataString.Replace("\"", "").Split(',');

            var fareId = data[mappings["fare_id"]];

            var routeId = mappings.ContainsKey("route_id") ? data[mappings["route_id"]] : null;
            var originId = mappings.ContainsKey("origin_id") ? data[mappings["origin_id"]] : null;
            var destinationId = mappings.ContainsKey("destination_id") ? data[mappings["destination_id"]] : null;
            var containsId = mappings.ContainsKey("contains_id") ? data[mappings["contains_id"]] : null;

            var fareRule = new FareRule
            {
                FareId = fareId,
                RouteId = routeId,
                OriginId = originId,
                DestinationId = destinationId,
                ContainsId = containsId
            };
            return fareRule;
        }
    }
}