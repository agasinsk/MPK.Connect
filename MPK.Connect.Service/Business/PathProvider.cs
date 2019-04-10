using System;
using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.Graph;

namespace MPK.Connect.Service.Business
{
    public class PathProvider : IPathProvider
    {
        private readonly IStopTimePathFinder _pathFinder;

        /// <summary>
        /// Initializes a new instance of the <see cref="PathProvider"/> class.
        /// </summary>
        /// <param name="pathFinder">The path finder.</param>
        /// <exception cref="ArgumentNullException">pathFinder</exception>
        public PathProvider(IStopTimePathFinder pathFinder)
        {
            _pathFinder = pathFinder ?? throw new ArgumentNullException(nameof(pathFinder));
        }

        /// <summary>
        /// Gets travel plans from source to destination
        /// </summary>
        /// <param name="graph">Graph with stop times</param>
        /// <param name="source">Source</param>
        /// <param name="destination">Destination</param>
        /// <returns>Collection of probable paths from source to destination</returns>
        public List<Path<StopTimeInfo>> GetAvailablePaths(Graph<int, StopTimeInfo> graph, Location source,
            Location destination)
        {
            var referentialDestination = graph.GetReferenceDestinationStop(destination.Name);
            var sources = graph.GetSourceNodes(source.Name, referentialDestination);

            // Search for shortest path from subsequent sources to destination
            var paths = sources
                .Select(s => _pathFinder.FindShortestPath(graph, s.Data, referentialDestination))
                .Where(path => path.Any())
                .ToList();

            var filteredPaths = paths
                .Distinct(new PathComparer())
                .OrderBy(p => p.First().DepartureTime)
                .ThenBy(p => p.Cost)
                .ToList();

            return filteredPaths;
        }
    }
}