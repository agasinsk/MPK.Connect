using System;
using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model.Graph;

namespace MPK.Connect.Service.Business.Graph
{
    public static class StopGraphExtensions
    {
        public static IEnumerable<StopGraphEdge> FindShortestPath(this Dictionary<string, StopGraphNode> graph, string source, string destination)
        {
            var now = DateTime.Now.TimeOfDay;
            // Initialize distance and route tables
            var distances = new Dictionary<string, int>();
            var routes = new Dictionary<string, StopGraphEdge>();

            foreach (var stopId in graph.Keys)
            {
                distances[stopId] = int.MaxValue;
                routes[stopId] = null;
            }

            distances[source] = 0;
            var tripTime = 0;
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
                    var maxTime = now + TimeSpan.FromMinutes(tripTime);
                    var neighborsaftertime = minimumNode.Neighbors.Where(n => n.DepartureTime > maxTime).ToList();

                    // relax each edge
                    foreach (var neighbor in minimumNode.Neighbors)
                    {
                        var distTouCity = distances[minimumStopId];
                        var distTovCity = distances[neighbor.DestinationStopId];

                        if (distTovCity > distTouCity + neighbor.Cost)
                        {
                            // update distance and route
                            distances[neighbor.DestinationStopId] = distTouCity + neighbor.Cost;
                            tripTime = distTouCity + neighbor.Cost;
                            routes[neighbor.DestinationStopId] = neighbor;
                        }
                    }
                }
            }
            /**** END DIJKSTRA ****/

            // Track the path
            var traceBackSteps = new List<StopGraphEdge>();
            traceBackSteps.Add(new StopGraphEdge
            {
                SourceStopId = destination,
                DestinationStopId = destination
            });
            var currentStopId = destination;
            do
            {
                var currentNodeEdge = routes[currentStopId];
                currentStopId = currentNodeEdge.SourceStopId;
                traceBackSteps.Add(currentNodeEdge);
            } while (currentStopId != source);

            traceBackSteps.Reverse();
            return traceBackSteps;
        }
    }
}