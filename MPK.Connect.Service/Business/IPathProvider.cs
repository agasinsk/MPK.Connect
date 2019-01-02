using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using System.Collections.Generic;

namespace MPK.Connect.Service.Business
{
    public interface IPathProvider
    {
        List<Path<StopTimeInfo>> GetAvailablePaths(Graph<int, StopTimeInfo> graph, Location sourceLocation,
            Location destinationLocation);
    }
}