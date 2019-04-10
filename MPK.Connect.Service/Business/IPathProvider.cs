using System.Collections.Generic;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;

namespace MPK.Connect.Service.Business
{
    public interface IPathProvider
    {
        /// <summary>
        /// Gets the available paths.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source location.</param>
        /// <param name="destination">The destination location.</param>
        /// <returns></returns>
        List<Path<StopTimeInfo>> GetAvailablePaths(Graph<int, StopTimeInfo> graph, Location source,
            Location destination);
    }
}