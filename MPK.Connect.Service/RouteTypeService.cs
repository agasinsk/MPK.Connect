using System.Collections.Generic;

using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess.Routes.Types;
using MPK.Connect.Model;

namespace MPK.Connect.Service
{
    public class RouteTypeService : GenericService<RouteType>
    {
        public RouteTypeService(IRouteTypeRepository routeRepository, ILogger<RouteTypeService> logger) : base(
            routeRepository, logger)
        {
        }

        protected override RouteType Map(string entityString)
        {
            var routeTypeInfos = entityString.Split(',');
            var id = int.Parse(routeTypeInfos[0]);
            var name = routeTypeInfos[1];
            var routeType = new RouteType
            {
                RouteTypeId = id,
                Name = name
            };
            return routeType;
        }

        protected override void SortEntities(List<RouteType> entities)
        {
        }
    }
}