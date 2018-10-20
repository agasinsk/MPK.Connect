using MPK.Connect.Model;

namespace MPK.Connect.Service.Builders
{
    public class FareRuleBuilder : BaseEntityBuilder<FareRule>
    {
        public override FareRule Build(string dataString)
        {
            var data = dataString.Replace("\"", "").Split(',');

            var fareId = data[_entityMappings["fare_id"]];

            var routeId = _entityMappings.ContainsKey("route_id") ? data[_entityMappings["route_id"]] : null;
            var originId = _entityMappings.ContainsKey("origin_id") ? data[_entityMappings["origin_id"]] : null;
            var destinationId = _entityMappings.ContainsKey("destination_id") ? data[_entityMappings["destination_id"]] : null;
            var containsId = _entityMappings.ContainsKey("contains_id") ? data[_entityMappings["contains_id"]] : null;

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