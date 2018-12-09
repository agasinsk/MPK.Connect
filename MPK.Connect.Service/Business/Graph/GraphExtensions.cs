using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;

namespace MPK.Connect.Service.Business.Graph
{
    public static class GraphExtensions
    {
        public static IEnumerable<T> A_Star<TId, T>(this Graph<TId, T> graph, T source, T destination)
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
            var costFromSourceToNode =
                new Dictionary<TId, double>(dictionary)
                {
                    [source.Id] = 0
                };

            var totalCostFromSourceToNode =
                new Dictionary<TId, double>(graph.Nodes.ToDictionary(k => k.Key, v => double.MaxValue))
                {
                    [source.Id] = source.GetDistanceTo(destination)
                };

            while (nodesToExtend.Any())
            {
                var nodeWithLowestCostId = nodesToExtend.Aggregate((l, r) => totalCostFromSourceToNode[l.Key] < totalCostFromSourceToNode[r.Key] ? l : r).Key;
                if (nodeWithLowestCostId.Equals(destination.Id))
                {
                    // reconstruct path
                    var totalPath = new List<T>();
                    var currentNodeId = nodeWithLowestCostId;
                    while (cameFrom.ContainsKey(currentNodeId))
                    {
                        var node = cameFrom[currentNodeId];
                        currentNodeId = node.Id;
                        totalPath.Add(node.Data);
                    }

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
                    var tentative_gScore = costFromSourceToNode[currentNode.Id] + neighborEdge.Cost;

                    if (!nodesToExtend.ContainsKey(neighbor.Id))
                    {
                        nodesToExtend.Add(neighbor.Id, neighbor);
                    }
                    else if (tentative_gScore >= costFromSourceToNode[neighbor.Id])
                    {
                        continue;
                    }

                    // This path is the best until now. Record it!
                    cameFrom[neighbor.Id] = currentNode;
                    costFromSourceToNode[neighbor.Id] = tentative_gScore;

                    totalCostFromSourceToNode[neighbor.Id] =
                        costFromSourceToNode[neighbor.Id] + neighbor.Data.GetDistanceTo(destination);
                }
            }

            return new List<T>();
        }
    }
}