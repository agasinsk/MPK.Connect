using MPK.Connect.Model.Graph;
using System.Collections.Generic;

namespace MPK.Connect.Service.Business.Graph
{
    public static class StopGraphExtensions
    {
        public static IEnumerable<StopGraphNode> FindShortestPath(this Dictionary<string, StopGraphNode> graph, string source,
            string destination)
        {
            // Initialize distance and route tables
            var distances = new Dictionary<string, int>();
            var routes = new Dictionary<string, StopGraphNode>();

            foreach (var stopId in graph.Keys)
            {
                distances[stopId] = int.MaxValue;
                routes[stopId] = null;
            }

            distances[source] = 0;

            var stopIds = new List<string>(graph.Keys);	// nodes == Q

            /**** START DIJKSTRA ****/
            while (stopIds.Count > 0)
            {
                // get the minimum node
                var minDist = int.MaxValue;
                string minimumStopId = null;
                foreach (var stopId in stopIds)
                {
                    if (distances[stopId] <= minDist)
                    {
                        minDist = distances[stopId];
                        minimumStopId = stopId;
                    }
                }

                // remove it from the set Q
                stopIds.Remove(minimumStopId);

                // iterate through all of u's neighbors
                var minimumNode = graph[minimumStopId];
                if (minimumNode.Neighbors != null)
                {
                    // relax each edge
                    foreach (var neighbor in minimumNode.Neighbors)
                    {
                        var distTouCity = distances[minimumStopId];
                        var distTovCity = distances[neighbor.StopId];

                        if (distTovCity > distTouCity + neighbor.Cost)
                        {
                            // update distance and route
                            distances[neighbor.StopId] = distTouCity + neighbor.Cost;
                            routes[neighbor.StopId] = minimumNode;
                        }
                    }
                }
            }
            /**** END DIJKSTRA ****/

            // Track the path
            var traceBackSteps = new List<StopGraphNode>();
            var destinationNode = graph[destination];
            traceBackSteps.Add(destinationNode);
            var currentNode = destinationNode;
            do
            {
                currentNode = routes[currentNode.Stop.Id];
                traceBackSteps.Add(currentNode);
            } while (currentNode.Stop.Id != source);

            traceBackSteps.Reverse();
            return traceBackSteps;
        }
    }
}