using MPK.Connect.Model;

namespace MPK.Connect.DataAccess.Routes.Types
{
    public interface IRouteTypeRepository : IGenericRepository<RouteType>
    {
        RouteType GetRouteType(int routeTypeId);
    }
}