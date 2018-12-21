using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Helpers;

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
            var probableDestination = graph.Nodes.Values
                .Where(n => n.Data.StopDto.Name.TrimToLower() == destination.TrimToLower() &&
                            n.Data.DepartureTime > source.DepartureTime)
                .OrderByDescending(n => n.Data.DepartureTime)
                .FirstOrDefault()?.Data;

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

            var totalCostToDestination =
                new Dictionary<string, double>(graph.Nodes.ToDictionary(k => k.Key, v => double.MaxValue))
                {
                    [source.Id] = source.GetDistanceTo(probableDestination) * 10
                };

            while (nodesToExtend.Any())
            {
                var orderedNodes = nodesToExtend.ToDictionary(k => k.Value, v => totalCostToDestination[v.Key]).OrderBy(n => n.Value).ToList();

                var nodeWithLowestCostId = nodesToExtend.Aggregate((l, r) => totalCostToDestination[l.Key] < totalCostToDestination[r.Key] ? l : r).Key;
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

                foreach (var neighborEdge in currentNode.Neighbors.OrderBy(n => n.Cost))
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
                    else if (costFromSource[neighbor.Id] <= tentative_gScore)
                    {
                        continue;
                    }

                    // This path is the best until now. Record it!
                    cameFrom[neighbor.Id] = currentNode;
                    costFromSource[neighbor.Id] = tentative_gScore;

                    var heuristic = neighbor.Data.GetDistanceTo(probableDestination) * 10;
                    totalCostToDestination[neighbor.Id] = costFromSource[neighbor.Id] + heuristic;
                }
            }

            return new Path<StopTimeInfo>();
        }

        public static Path<StopTimeInfo> AStarSecond(this Graph<string, StopTimeInfo> graph, StopTimeInfo source, string destination)
        {
            var probableDestination = graph.Nodes.Values
                .Where(n => n.Data.StopDto.Name.TrimToLower() == destination.TrimToLower() &&
                            n.Data.DepartureTime > source.DepartureTime)
                .OrderByDescending(n => n.Data.DepartureTime)
                .FirstOrDefault()?.Data;

            var closedSet = new Dictionary<string, GraphNode<string, StopTimeInfo>>();
            var openSet = new Dictionary<string, GraphNode<string, StopTimeInfo>>
            {
                { source.Id, graph.Nodes[source.Id] }
            };
            var g_score = new Dictionary<string, double>(graph.Nodes.ToDictionary(k => k.Key, v => double.MaxValue)) { [source.Id] = 0 };
            var f_score = new Dictionary<string, double>(graph.Nodes.ToDictionary(k => k.Key, v => double.MaxValue));
            var h_score = new Dictionary<string, double>
            {
                [source.Id] = source.GetDistanceTo(probableDestination)
            };
            var cameFrom = new Dictionary<string, GraphNode<string, StopTimeInfo>>();

            while (openSet.Any())
            {
                // the node in openset having the lowest f_score[] value
                var x = openSet.Aggregate((l, r) => f_score[l.Key] < f_score[r.Key] ? l : r);
                if (x.Value.Data.StopDto.Name.TrimToLower() == destination.TrimToLower())
                {
                    // reconstruct path
                    var totalPath = new Path<StopTimeInfo>();

                    var currentNodeId = x.Key;
                    var destinationNode = graph.Nodes[currentNodeId];
                    totalPath.Add(destinationNode.Data);
                    while (cameFrom.ContainsKey(currentNodeId))
                    {
                        var node = cameFrom[currentNodeId];
                        currentNodeId = node.Id;
                        totalPath.Add(node.Data);
                    }

                    totalPath.Reverse();
                    totalPath.Cost = g_score[probableDestination.Id];
                    return totalPath;
                }

                openSet.Remove(x.Key);
                closedSet.Add(x.Key, x.Value);

                foreach (var y in x.Value.Neighbors)
                {
                    if (closedSet.ContainsKey(y.DestinationId))
                    {
                        continue;
                    }

                    var tentative_g_score = g_score[x.Key] + y.Cost;
                    var tentativeIsBetter = false;

                    if (!openSet.ContainsKey(y.DestinationId))
                    {
                        var yNode = graph.Nodes[y.DestinationId];
                        openSet.Add(yNode.Id, yNode);
                        h_score[y.DestinationId] = yNode.Data.GetDistanceTo(probableDestination);
                        tentativeIsBetter = true;
                    }
                    else if (tentative_g_score < g_score[y.DestinationId])
                    {
                        tentativeIsBetter = true;
                    }

                    if (tentativeIsBetter)
                    {
                        cameFrom[y.DestinationId] = x.Value;
                        g_score[y.DestinationId] = tentative_g_score;
                        f_score[y.DestinationId] = g_score[y.DestinationId] + h_score[y.DestinationId];
                    }
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

        //function A*(start, goal)
        //    closedset := the empty set                 % Zbiór wierzchołków przejrzanych.
        //    openset := set containing the initial node % Zbiór wierzchołków nieodwiedzonych, sąsiadujących z odwiedzonymi.
        //    g_score[start] := 0                        % Długość optymalnej trasy.
        //while openset is not empty
        //x := the node in openset having the lowest f_score[] value
        //if x = goal
        //return reconstruct_path(came_from, goal)
        //remove x from openset
        //add x to closedset
        //foreach y in neighbor_nodes(x)
        //    if y in closedset
        //continue
        //tentative_g_score := g_score[x] + dist_between(x, y)
        //tentative_is_better := false
        //if y not in openset
        //    add y to openset
        //    h_score[y] := heuristic_estimate_of_distance_to_goal_from(y)
        //tentative_is_better := true
        //elseif tentative_g_score<g_score[y]
        //tentative_is_better := true
        //if tentative_is_better = true
        //came_from[y] := x
        //    g_score[y] := tentative_g_score
        //    f_score[y] := g_score[y] + h_score[y] % Przewidywany dystans od startu do celu przez y.
        //return failure
    }
}