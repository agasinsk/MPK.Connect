using System;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;

namespace MPK.Connect.Service.Business.Graph
{
    /// <summary>
    /// The interface for graph builder
    /// </summary>
    public interface IGraphBuilder
    {
        /// <summary>
        /// Gets the graph.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="graphLimits">The graph limits.</param>
        /// <returns>Graph</returns>
        Graph<int, StopTimeInfo> GetGraph(DateTime startDate, CoordinateLimits graphLimits = null);
    }
}