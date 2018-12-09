using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Helpers;

namespace MPK.Connect.Service.Business.Graph
{
    public class PathFinder
    {
        private readonly Graph<string, Stop> _graph;

        public PathFinder(Graph<string, Stop> graph)
        {
            _graph = graph;
        }

        public IEnumerable<StopGraphEdge<string>> A_Star(Stop source, Stop destination)
        {
            // Initialize
            var nodesAlreadyExtended = new List<string>();
            var nodesToExtend = new List<string> { source.Id };

            var cameFrom = new Dictionary<string, GraphEdge<string>>();
            var costFromSourceToNode =
                new Dictionary<string, double>(_graph.Nodes.ToDictionary(k => k.Key, v => double.MaxValue))
                {
                    [source.Id] = 0
                };

            var totalCostFromSourceToNode =
                new Dictionary<string, double>(_graph.Nodes.ToDictionary(k => k.Key, v => double.MaxValue))
                {
                    [source.Id] = source.GetDistanceTo(destination)
                };

            while (nodesToExtend.Any())
            {
                var nodeWithLowestCostId = totalCostFromSourceToNode.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                if (nodeWithLowestCostId == destination.Id)
                {
                    return ReconstructPath(cameFrom, nodeWithLowestCostId);
                }

                var currentNode = _graph.Nodes[nodeWithLowestCostId];

                nodesToExtend.Remove(currentNode.Id);
                nodesAlreadyExtended.Add(currentNode.Id);

                foreach (var neighborEdge in currentNode.Neighbors)
                {
                    var neighbor = _graph.Nodes[neighborEdge.DestinationId];
                    // Ignore the neighbor which is already evaluated
                    if (nodesAlreadyExtended.Contains(neighbor.Id))
                    {
                        continue;
                    }

                    // The distance from start to a neighbor
                    var tentative_gScore = costFromSourceToNode[currentNode.Id] + neighborEdge.Cost;

                    if (!nodesToExtend.Contains(neighbor.Id))
                    {
                        nodesToExtend.Add(neighbor.Id);
                    }
                    else if (tentative_gScore >= costFromSourceToNode[neighbor.Id])
                    {
                        continue;
                    }

                    // This path is the best until now. Record it!
                    cameFrom[neighbor.Id] = neighborEdge;
                    costFromSourceToNode[neighbor.Id] = tentative_gScore;

                    totalCostFromSourceToNode[neighbor.Id] =
                        costFromSourceToNode[neighbor.Id] + neighbor.Data.GetDistanceTo(destination);
                }
            }

            return new List<StopGraphEdge<string>>();
        }

        private IEnumerable<StopGraphEdge<string>> ReconstructPath(Dictionary<string, GraphEdge<string>> cameFrom, string currentNodeId)
        {
            var totalPath = new List<StopGraphEdge<string>>();

            while (cameFrom.ContainsKey(currentNodeId))
            {
                var currentEdge = cameFrom[currentNodeId];
                currentNodeId = currentEdge.SourceId;
                totalPath.Add(currentEdge as StopGraphEdge<string>);
            }

            return totalPath;
        }
    }
}