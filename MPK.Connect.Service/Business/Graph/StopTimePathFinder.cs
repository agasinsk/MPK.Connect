using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Business.Graph
{
    /// <inheritdoc/>
    /// <summary>
    /// The stop path finder
    /// </summary>
    /// <seealso cref="T:MPK.Connect.Service.Business.Graph.IStopTimePathFinder"/>
    public class StopTimePathFinder : IStopTimePathFinder
    {
        /// <summary>
        /// The distance enhancement factor
        /// </summary>
        private const double DistanceEnhancementFactor = 10;

        /// <summary>
        /// Finds the shortest path using A* algorithm
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="destinationStop">Name of the destination.</param>
        /// <returns>Shortest path between source and destination</returns>
        public Path<StopTimeInfo> FindShortestPath(Graph<int, StopTimeInfo> graph, StopTimeInfo source, StopDto destinationStop)
        {
            // Initialize extended list
            var nodesAlreadyExtended = new List<GraphNode<int, StopTimeInfo>>();
            var nodesToExtend = new Dictionary<int, GraphNode<int, StopTimeInfo>>
            {
                { source.Id, graph.Nodes[source.Id] }
            };

            // Initialize distance lookups
            var cameFrom = new Dictionary<int, GraphNode<int, StopTimeInfo>>();
            var nodeDistances = graph.Nodes.ToDictionary(k => k.Key, v => double.MaxValue);
            var distanceFromSource = new Dictionary<int, double>(nodeDistances)
            {
                [source.Id] = 0
            };
            var totalDistanceToDestination = new Dictionary<int, double>(nodeDistances)
            {
                [source.Id] = source.StopDto.GetDistanceTo(destinationStop) * DistanceEnhancementFactor
            };

            // Look for paths
            while (nodesToExtend.Any())
            {
                // Get node with minimum distance
                var currentNodeWithMinimumDistance = nodesToExtend.Aggregate((l, r) => totalDistanceToDestination[l.Key] < totalDistanceToDestination[r.Key] ? l : r).Value;

                // If destination is reached
                if (currentNodeWithMinimumDistance.Data.StopDto.Name.TrimToLower() == destinationStop.Name.TrimToLower() ||
                    currentNodeWithMinimumDistance.Id.Equals(destinationStop?.Id))
                {
                    // Reconstruct path
                    return ReconstructPath(currentNodeWithMinimumDistance, graph, cameFrom, source.StopDto.Name);
                }

                nodesToExtend.Remove(currentNodeWithMinimumDistance.Id);
                nodesAlreadyExtended.Add(currentNodeWithMinimumDistance);

                foreach (var neighborEdge in currentNodeWithMinimumDistance.Neighbors)
                {
                    var neighbor = graph.Nodes[neighborEdge.DestinationId];

                    // Ignore the neighbor which is already extended
                    if (nodesAlreadyExtended.Contains(neighbor))
                    {
                        continue;
                    }

                    // Calculate the distance from start to a neighbor
                    var distanceToNeighbor = distanceFromSource[currentNodeWithMinimumDistance.Id] + neighborEdge.Cost;

                    if (!nodesToExtend.ContainsKey(neighbor.Id))
                    {
                        nodesToExtend.Add(neighbor.Id, neighbor);
                    }
                    // Skip neighbor if it is not better than the best minimum distance so far
                    else if (distanceToNeighbor >= distanceFromSource[neighbor.Id])
                    {
                        continue;
                    }

                    // This path is the best until now. Record it!
                    cameFrom[neighbor.Id] = currentNodeWithMinimumDistance;
                    distanceFromSource[neighbor.Id] = distanceToNeighbor;

                    var distanceFromNeighborToDestination = neighbor.Data.StopDto.GetDistanceTo(destinationStop) * DistanceEnhancementFactor;

                    totalDistanceToDestination[neighbor.Id] = distanceFromSource[neighbor.Id] + distanceFromNeighborToDestination;
                }
            }

            return new Path<StopTimeInfo>();
        }

        private Path<StopTimeInfo> ReconstructPath(GraphNode<int, StopTimeInfo> destinationNode,
            Graph<int, StopTimeInfo> graph, Dictionary<int, GraphNode<int, StopTimeInfo>> cameFrom, string sourceName)
        {
            var totalPath = new Path<StopTimeInfo>();

            // Add destination node
            var currentNode = graph.Nodes[destinationNode.Id];
            totalPath.Add(currentNode.Data);

            // Reconstruct path
            var currentNodeId = currentNode.Id;
            while (cameFrom.ContainsKey(currentNodeId))
            {
                currentNode = cameFrom[currentNodeId];
                currentNodeId = currentNode.Id;
                totalPath.Add(currentNode.Data);

                if (currentNode.Data.StopDto.Name.TrimToLower() == sourceName.TrimToLower())
                {
                    break;
                }
            }

            totalPath.Reverse();
            totalPath.Cost = (totalPath.Last().DepartureTime - totalPath.First().DepartureTime).TotalMinutes;

            return totalPath;
        }
    }
}