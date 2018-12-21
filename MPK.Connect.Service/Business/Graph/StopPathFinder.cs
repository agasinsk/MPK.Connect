using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Helpers;

namespace MPK.Connect.Service.Business.Graph
{
    public class StopPathFinder : IStopPathFinder
    {
        private const double _distanceEnhancementFactor = 10;

        /// <summary>
        /// Finds the shortest path using A* algorithm
        /// </summary>
        /// <param name="graph">Graph to be searched</param>
        /// <param name="source">Source node</param>
        /// <param name="destinationName">Name of the destination stop</param>
        /// <returns>Shortest path between source and destination</returns>
        public Path<StopTimeInfo> FindShortestPath(Graph<string, StopTimeInfo> graph, StopTimeInfo source, string destinationName)
        {
            var probableDestination = graph.Nodes.Values
                .Where(n => n.Data.StopDto.Name.TrimToLower() == destinationName.TrimToLower() &&
                            n.Data.DepartureTime > source.DepartureTime)
                .OrderByDescending(n => n.Data.DepartureTime)
                .FirstOrDefault()?.Data;

            // Initialize extended list
            var nodesAlreadyExtended = new List<GraphNode<string, StopTimeInfo>>();
            var nodesToExtend = new Dictionary<string, GraphNode<string, StopTimeInfo>>
            {
                { source.Id, graph.Nodes[source.Id] }
            };

            // Initialize distance lookups
            var cameFrom = new Dictionary<string, GraphNode<string, StopTimeInfo>>();
            var nodeDistances = graph.Nodes.ToDictionary(k => k.Key, v => double.MaxValue);
            var distanceFromSource = new Dictionary<string, double>(nodeDistances)
            {
                [source.Id] = 0
            };
            var totalDistanceToDestination = new Dictionary<string, double>(nodeDistances)
            {
                [source.Id] = source.GetDistanceTo(probableDestination) * _distanceEnhancementFactor
            };

            // Look for paths
            while (nodesToExtend.Any())
            {
                // Get node with minimum distance
                var currentNodeWithMinimumDistance = nodesToExtend.Aggregate((l, r) => totalDistanceToDestination[l.Key] < totalDistanceToDestination[r.Key] ? l : r).Value;

                // If destination is reached
                if (currentNodeWithMinimumDistance.Data.StopDto.Name.TrimToLower() == destinationName.TrimToLower() ||
                    currentNodeWithMinimumDistance.Id.Equals(probableDestination?.Id))
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
                    var distanceFromNeighborToDestination = neighbor.Data.GetDistanceTo(probableDestination) * _distanceEnhancementFactor;
                    totalDistanceToDestination[neighbor.Id] = distanceFromSource[neighbor.Id] + distanceFromNeighborToDestination;
                }
            }

            return new Path<StopTimeInfo>();
        }

        private static Path<StopTimeInfo> ReconstructPath(GraphNode<string, StopTimeInfo> destinationNode, Graph<string, StopTimeInfo> graph, Dictionary<string, GraphNode<string, StopTimeInfo>> cameFrom, string sourceName)
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
            totalPath.Cost = (totalPath.Last().ArrivalTime - totalPath.First().DepartureTime).TotalMinutes;
            return totalPath;
        }
    }
}