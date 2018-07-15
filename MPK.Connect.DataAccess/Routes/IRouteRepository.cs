using MPK.Connect.Model;

namespace MPK.Connect.DataAccess.Routes
{
    public interface IRouteRepository : IGenericRepository<Route>
    {
        Route GetRoute(string routeId);
    }
}