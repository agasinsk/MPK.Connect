using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;

namespace MPK.Connect.Service.Business
{
    public class PathComparer : IEqualityComparer<Path<StopTimeInfo>>
    {
        public bool Equals(Path<StopTimeInfo> x, Path<StopTimeInfo> y)
        {
            if (!x.Any() && !y.Any() && x.Cost.Equals(y.Cost))
            {
                return true;
            }

            if (x.Any() && !y.Any() || !x.Any() && y.Any())
            {
                return false;
            }
            var isDepartureTimeTheSame = x.First().DepartureTime == y.First().DepartureTime;

            var routesX = x.Select(s => s.Route).Distinct();
            var routesY = y.Select(s => s.Route).Distinct();
            var areRoutesTheSame = routesX.SequenceEqual(routesY);

            return isDepartureTimeTheSame && areRoutesTheSame;
        }

        public int GetHashCode(Path<StopTimeInfo> obj)
        {
            var routesHashCode = (int)obj.Select(s => s.Route).Distinct().Average(r => r.GetHashCode());
            return obj.First().DepartureTime.GetHashCode() + routesHashCode;
        }
    }
}