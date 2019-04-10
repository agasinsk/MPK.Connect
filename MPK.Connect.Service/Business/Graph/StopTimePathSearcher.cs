using System;
using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Business.Graph
{
    public class StopTimePathSearcher : IPathSearcher<StopTimeInfo>
    {
        private readonly Graph<int, StopTimeInfo> _graph;
        private readonly IStopTimePathFinder _pathFinder;
        private readonly StopDto _referentialDestinationStop;
        private readonly List<GraphNode<int, StopTimeInfo>> _sourceNodes;

        public Location Destination { get; }

        public Location Source { get; }

        public StopTimePathSearcher(IStopTimePathFinder pathFinder, Graph<int, StopTimeInfo> graph, Location source, Location destination)
        {
            _pathFinder = pathFinder ?? throw new ArgumentNullException(nameof(pathFinder));

            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));
            Source = source ?? throw new ArgumentNullException(nameof(source));

            // Set up source and destination nodes
            _referentialDestinationStop = graph.GetReferenceDestinationStop(Destination.Name);
            _sourceNodes = graph.GetSourceNodes(Source.Name, _referentialDestinationStop);
        }

        /// <summary>
        /// Gets the available paths.
        /// </summary>
        /// <returns>Collection of available paths</returns>
        public IEnumerable<Harmony<StopTimeInfo>> GetAvailablePaths()
        {
            // Search for shortest path from subsequent sources to destination
            var paths = _sourceNodes
                .Select(s => _pathFinder.FindShortestPath(_graph, s.Data, _referentialDestinationStop))
                .Where(path => path.Any())
                .ToList();

            return paths
                .Distinct(new PathComparer())
                .Select(p => new Harmony<StopTimeInfo>(p.Cost, p.ToArray()))
                .ToList();
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <returns></returns>
        public Harmony<StopTimeInfo> GetShortestPath()
        {
            var sourceNode = _sourceNodes.GetRandomElement();

            var shortestPath = _pathFinder.FindShortestPath(_graph, sourceNode.Data, _referentialDestinationStop);

            return new Harmony<StopTimeInfo>(shortestPath.Cost, shortestPath.ToArray());
        }
    }
}