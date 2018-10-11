using MPK.Connect.Model;
using System.Linq;

namespace MPK.Connect.DataAccess.Routes
{
    public class RouteRepository : GenericRepository<MpkContext, Route>, IRouteRepository
    {
        public Route GetRoute(string routeId)
        {
            return GetAll().FirstOrDefault(r => r.Id == routeId);
        }
    }
}