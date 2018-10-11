using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess.Routes;
using MPK.Connect.Model;
using System.Collections.Generic;

namespace MPK.Connect.Service
{
    public class RouteService : ImporterService<Route>
    {
        public RouteService(IRouteRepository stopsRepository, ILogger<RouteService> logger) : base(stopsRepository, logger)
        {
        }

        protected override Route Map(string entityString)
        {
            var routeInfos = entityString.Replace("\"", "").Split(',');
            var routeId = routeInfos[0];
            var agencyId = routeInfos[0];

            var mappedStop = new Route
            {
                Id = routeId,
            };

            return mappedStop;
        }

        protected override void SortEntities(List<Route> entities)
        {
        }
    }
}