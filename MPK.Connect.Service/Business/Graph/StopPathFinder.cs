using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;

namespace MPK.Connect.Service.Business.Graph
{
    public class StopPathFinder : IStopPathFinder
    {
        public Path<StopTimeInfo> FindShortestPath(Graph<string, StopTimeInfo> graph, StopTimeInfo source, string destinationName)
        {
            var probableDestination = graph.Nodes.Values
                    .Where(n => n.Data.Stop.Name.Trim().ToLower().Contains(destinationName.Trim().ToLower()) && n.Data.DepartureTime > source.DepartureTime)
                .OrderByDescending(n => n.Data.DepartureTime)
                .FirstOrDefault().Data;

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
            var totalDistanceFromSource = new Dictionary<string, double>(nodeDistances)
            {
                [source.Id] = source.GetDistanceTo(probableDestination)
            };

            // Look for paths
            while (nodesToExtend.Any())
            {
                // Get node with minimum distance
                var currentNodeWithMinimumDistance = nodesToExtend.Aggregate((l, r) => totalDistanceFromSource[l.Key] < totalDistanceFromSource[r.Key] ? l : r).Value;

                // Reconstruct path if destination is reached
                if (currentNodeWithMinimumDistance.Data.Stop.Name.Trim().ToLower().Contains(destinationName.Trim().ToLower()) || currentNodeWithMinimumDistance.Id.Equals(probableDestination.Id))
                {
                    // reconstruct path
                    var totalPath = new Path<StopTimeInfo>();

                    var currentNodeId = currentNodeWithMinimumDistance.Id;
                    var destinationNode = graph.Nodes[currentNodeId];
                    totalPath.Add(destinationNode.Data);
                    while (cameFrom.ContainsKey(currentNodeId))
                    {
                        var node = cameFrom[currentNodeId];
                        currentNodeId = node.Id;
                        totalPath.Add(node.Data);
                    }

                    totalPath.Reverse();
                    totalPath.Cost = distanceFromSource[totalPath.Last().Id];
                    return totalPath;
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
                    totalDistanceFromSource[neighbor.Id] = distanceFromSource[neighbor.Id] + neighbor.Data.GetDistanceTo(probableDestination);
                }
            }

            return new Path<StopTimeInfo>();
        }

        /// <summary>
        /// Reconstructs path from destination to source
        /// </summary>
        /// <typeparam name="TId">Type of id</typeparam>
        /// <typeparam name="T">Type of graph node values</typeparam>
        /// <param name="destination">Destination node</param>
        /// <param name="cameFrom">Collection of associations between subsequent graph nodes</param>
        /// <returns>Path from source to destination</returns>
        private IEnumerable<TId> ReconstructPath<TId, T>(T destination, Dictionary<TId, GraphNode<TId, T>> cameFrom)
            where TId : class
            where T : LocalizableEntity<TId>
        {
            var path = new List<TId> { destination.Id };
            var currentId = destination.Id;
            while (cameFrom.ContainsKey(currentId))
            {
                var node = cameFrom[currentId];
                currentId = node.Id;
                path.Add(node.Id);
            }

            path.Reverse();
            return path;
        }
    }
}