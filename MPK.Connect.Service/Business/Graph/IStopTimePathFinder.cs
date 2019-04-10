using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;

namespace MPK.Connect.Service.Business.Graph
{
    /// <summary>
    /// The interface for stop path finder
    /// </summary>
    public interface IStopTimePathFinder
    {
        /// <summary>
        /// Finds the shortest path using A* algorithm
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="destinationStop">Name of the destination.</param>
        /// <returns></returns>
        Path<StopTimeInfo> FindShortestPath(Graph<int, StopTimeInfo> graph, StopTimeInfo source,
            StopDto destinationStop);
    }
}