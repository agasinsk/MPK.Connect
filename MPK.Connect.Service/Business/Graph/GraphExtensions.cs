using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;

namespace MPK.Connect.Service.Business.Graph
{
    public static class GraphExtensions
    {
        public static Path<T> A_Star<TId, T>(this Graph<TId, T> graph, T source, T destination)
            where TId : class
            where T : LocalizableEntity<TId>
        {
            // Initialize extended list
            var nodesAlreadyExtended = new List<GraphNode<TId, T>>();
            var nodesToExtend = new Dictionary<TId, GraphNode<TId, T>>
            {
                { source.Id, graph.Nodes[source.Id] }
            };

            // Initialize distance lookups
            var cameFrom = new Dictionary<TId, GraphNode<TId, T>>();
            var nodeDistances = graph.Nodes.ToDictionary(k => k.Key, v => double.MaxValue);
            var distanceFromSource = new Dictionary<TId, double>(nodeDistances)
            {
                [source.Id] = 0
            };
            var totalDistanceFromSource = new Dictionary<TId, double>(nodeDistances)
            {
                [source.Id] = source.GetDistanceTo(destination)
            };

            // Look for paths
            while (nodesToExtend.Any())
            {
                // Get node with minimum
                var currentNodeWithMinimumDistance = nodesToExtend.Aggregate((l, r) => totalDistanceFromSource[l.Key] < totalDistanceFromSource[r.Key] ? l : r).Value;

                // Reconstruct path if destination is reached
                if (currentNodeWithMinimumDistance.Id.Equals(destination.Id))
                {
                    var pathIds = ReconstructPath(destination, cameFrom);
                    var nodes = pathIds.Select(n => graph.Nodes[n].Data).ToList();
                    var totalPath = new Path<T>();
                    totalPath.AddRange(nodes);
                    totalPath.Cost = distanceFromSource[destination.Id];
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
                    totalDistanceFromSource[neighbor.Id] = distanceFromSource[neighbor.Id] + neighbor.Data.GetDistanceTo(destination);
                }
            }

            return new Path<T>();
        }

        public static Path<StopTimeInfo> AStar(this Graph<string, StopTimeInfo> graph, StopTimeInfo source, string destination)
        {
            var probableDestination = graph.Nodes.Values.FirstOrDefault(n => n.Data.StopDto.Name.Trim().ToLower().Contains(destination.Trim().ToLower()))?.Data;

            // Initialize
            var nodesAlreadyExtended = new List<GraphNode<string, StopTimeInfo>>();
            var nodesToExtend = new Dictionary<string, GraphNode<string, StopTimeInfo>>
            {
                { source.Id, graph.Nodes[source.Id] }
            };

            var cameFrom = new Dictionary<string, GraphNode<string, StopTimeInfo>>();
            var dictionary = graph.Nodes.ToDictionary(k => k.Key, v => double.MaxValue);
            var costFromSource =
                new Dictionary<string, double>(dictionary)
                {
                    [source.Id] = 0
                };

            var totalCostFromSource =
                new Dictionary<string, double>(graph.Nodes.ToDictionary(k => k.Key, v => double.MaxValue))
                {
                    [source.Id] = source.GetDistanceTo(probableDestination)
                };

            while (nodesToExtend.Any())
            {
                var nodeWithLowestCostId = nodesToExtend.Aggregate((l, r) => totalCostFromSource[l.Key] < totalCostFromSource[r.Key] ? l : r).Key;
                var currentNode = graph.Nodes[nodeWithLowestCostId];
                if (currentNode.Data.StopDto.Name.Trim().ToLower().Contains(destination.Trim().ToLower()))
                {
                    // reconstruct path
                    var totalPath = new Path<StopTimeInfo>();

                    var currentNodeId = nodeWithLowestCostId;
                    var destinationNode = graph.Nodes[currentNodeId];
                    totalPath.Add(destinationNode.Data);
                    while (cameFrom.ContainsKey(currentNodeId))
                    {
                        var node = cameFrom[currentNodeId];
                        currentNodeId = node.Id;
                        totalPath.Add(node.Data);
                    }

                    totalPath.Reverse();
                    totalPath.Cost = costFromSource[probableDestination.Id];
                    return totalPath;
                }

                nodesToExtend.Remove(currentNode.Id);
                nodesAlreadyExtended.Add(currentNode);

                foreach (var neighborEdge in currentNode.Neighbors)
                {
                    var neighbor = graph.Nodes[neighborEdge.DestinationId];
                    // Ignore the neighbor which is already evaluated
                    if (nodesAlreadyExtended.Contains(neighbor))
                    {
                        continue;
                    }

                    // The distance from start to a neighbor
                    var tentative_gScore = costFromSource[currentNode.Id] + neighborEdge.Cost;

                    if (!nodesToExtend.ContainsKey(neighbor.Id))
                    {
                        nodesToExtend.Add(neighbor.Id, neighbor);
                    }
                    else if (tentative_gScore >= costFromSource[neighbor.Id])
                    {
                        continue;
                    }

                    // This path is the best until now. Record it!
                    cameFrom[neighbor.Id] = currentNode;
                    costFromSource[neighbor.Id] = tentative_gScore;

                    totalCostFromSource[neighbor.Id] =
                        costFromSource[neighbor.Id] + neighbor.Data.GetDistanceTo(probableDestination);
                }
            }

            return new Path<StopTimeInfo>();
        }

        private static IEnumerable<TId> ReconstructPath<TId, T>(T destination, Dictionary<TId, GraphNode<TId, T>> cameFrom)
            where TId : class
            where T : LocalizableEntity<TId>
        {
            // reconstruct path
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