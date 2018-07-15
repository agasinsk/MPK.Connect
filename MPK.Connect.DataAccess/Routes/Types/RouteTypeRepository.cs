using System.Linq;
using MPK.Connect.Model;

namespace MPK.Connect.DataAccess.Routes.Types
{
    public class RouteTypeRepository : GenericRepository<MpkContext, RouteType>, IRouteTypeRepository
    {
        public RouteType GetRouteType(int routeTypeId)
        {
            return GetAll().FirstOrDefault(r => r.Id == routeTypeId);
        }
    }
}