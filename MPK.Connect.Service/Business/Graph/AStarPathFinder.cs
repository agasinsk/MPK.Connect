using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;

namespace MPK.Connect.Service.Business.Graph
{
    public class AStarPathFinder : IPathFinder
    {
        /// <summary>
        /// Finds the shortest path using A* algorithm
        /// </summary>
        /// <typeparam name="TId">Type of id</typeparam>
        /// <typeparam name="T">Type of graph nodes</typeparam>
        /// <param name="graph">Graph to be searched</param>
        /// <param name="source">Source node</param>
        /// <param name="destination">Destination</param>
        /// <returns>Shortest path between source and destination</returns>
        public Path<T> FindShortestPath<TId, T>(Graph<TId, T> graph, T source, T destination) where TId : class where T : LocalizableEntity<TId>
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
                // Get node with minimum distance
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

        /// <summary>
        /// Finds the shortest path using A* algorithm
        /// </summary>
        /// <typeparam name="TId">Type of id</typeparam>
        /// <typeparam name="T">Type of graph nodes</typeparam>
        /// <param name="graph">Graph to be searched</param>
        /// <param name="source">Source node</param>
        /// <param name="destination">Destination</param>
        /// <param name="destinationName">Name of destination</param>
        /// <returns>Shortest path between source and destination</returns>
        public Path<T> FindShortestPath<TId, T>(Graph<TId, T> graph, T source, T destination, string destinationName = null)
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
                // Get node with minimum distance
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