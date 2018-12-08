using System;
using System.Collections.Generic;
using MPK.Connect.Model.Graph;

namespace MPK.Connect.Service.Business.Graph
{
    public static class StopGraphExtensions
    {
        public static IEnumerable<StopGraphNode> FindShortestPath(this Dictionary<string, StopGraphNode> graph, string source,
            string destination)
        {
            // Initialize distance and route tables
            var distances = new Dictionary<string, TravelCost>();
            var routes = new Dictionary<string, StopGraphNode>();

            foreach (var stopId in graph.Keys)
            {
                distances[stopId] = new TravelCost { Cost = int.MaxValue, DepartureTime = TimeSpan.MaxValue };
                routes[stopId] = null;
            }

            distances[source] = new TravelCost(0, DateTime.Now.TimeOfDay);

            var stopIds = new List<string>(graph.Keys);	// nodes == Q

            /**** START DIJKSTRA ****/
            while (stopIds.Count > 0)
            {
                // get the minimum node
                var minDist = int.MaxValue;
                string minimumStopId = null;
                foreach (var stopId in stopIds)
                {
                    if (distances[stopId].Cost <= minDist)
                    {
                        minDist = distances[stopId].Cost;
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

                        if (distTovCity.Cost > distTouCity.Cost + neighbor.Cost && distTovCity.DepartureTime > neighbor.DepartureTime)
                        {
                            // update distance and route
                            distances[neighbor.StopId] = new TravelCost(distTouCity.Cost + neighbor.Cost, neighbor.DepartureTime);
                            routes[neighbor.StopId] = new StopGraphNode(minimumNode.Stop, new List<StopGraphEdge> { neighbor });
                        }
                    }
                }
            }
            /**** END DIJKSTRA ****/

            // Track the path
            var traceBackSteps = new List<StopGraphNode>();
            var destinationNode = graph[destination];
            traceBackSteps.Add(new StopGraphNode(destinationNode.Stop));
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