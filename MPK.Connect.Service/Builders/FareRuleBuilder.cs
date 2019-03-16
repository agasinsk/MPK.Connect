using MPK.Connect.Model;
using MPK.Connect.Service.Helpers;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Builders
{
    public class FareRuleBuilder : BaseEntityBuilder<FareRule>
    {
        public override FareRule Build(string dataString)
        {
            var data = dataString.Replace("\"", "").ToEntityData();

            var fareId = data[_entityMappings["fare_id"]];
            var routeId = data[_entityMappings["route_id"]];
            var originId = data[_entityMappings["origin_id"]];
            var destinationId = data[_entityMappings["destination_id"]];
            var containsId = data[_entityMappings["contains_id"]];

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