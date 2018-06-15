using System.Collections.Generic;
using MPK.Connect.Model;

namespace MPK.Connect.DataAccess.Routes
{
    public interface IRouteRepository
    {
        Route CreateRoute(Route route);

        List<Route> CreateRoutes(List<Route> routes);

        Route DeleteRoute(Route route);

        Route GetRoute(string routeId);

        Route UpdateRoute(Route route);
    }
}