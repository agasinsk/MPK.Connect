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

            var mappedEntity = new Route
            {
            };

            return mappedEntity;
        }

        protected override void SortEntities(List<Route> entities)
        {
        }
    }
}