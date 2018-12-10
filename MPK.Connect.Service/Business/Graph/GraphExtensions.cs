using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;
using System.Collections.Generic;
using System.Linq;

namespace MPK.Connect.Service.Business.Graph
{
    public static class GraphExtensions
    {
        public static Path<T> A_Star<TId, T>(this Graph<TId, T> graph, T source, T destination)
            where T : LocalizableEntity<TId> where TId : class
        {
            // Initialize
            var nodesAlreadyExtended = new List<GraphNode<TId, T>>();
            var nodesToExtend = new Dictionary<TId, GraphNode<TId, T>>
            {
                { source.Id, graph.Nodes[source.Id] }
            };

            var cameFrom = new Dictionary<TId, GraphNode<TId, T>>();
            var dictionary = graph.Nodes.ToDictionary(k => k.Key, v => double.MaxValue);
            var costFromSource =
                new Dictionary<TId, double>(dictionary)
                {
                    [source.Id] = 0
                };

            var totalCostFromSource =
                new Dictionary<TId, double>(graph.Nodes.ToDictionary(k => k.Key, v => double.MaxValue))
                {
                    [source.Id] = source.GetDistanceTo(destination)
                };

            while (nodesToExtend.Any())
            {
                var nodeWithLowestCostId = nodesToExtend.Aggregate((l, r) => totalCostFromSource[l.Key] < totalCostFromSource[r.Key] ? l : r).Key;
                if (nodeWithLowestCostId.Equals(destination.Id))
                {
                    // reconstruct path
                    var totalPath = new Path<T>();

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
                    totalPath.Cost = costFromSource[destination.Id];
                    return totalPath;
                }

                var currentNode = graph.Nodes[nodeWithLowestCostId];

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
                        costFromSource[neighbor.Id] + neighbor.Data.GetDistanceTo(destination);
                }
            }

            return new Path<T>();
        }
    }
}