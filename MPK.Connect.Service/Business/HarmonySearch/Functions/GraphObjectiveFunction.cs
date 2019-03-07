using System;
using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Helpers;

namespace MPK.Connect.Service.Business.HarmonySearch.Functions
{
    public class GraphObjectiveFunction : IGeneralObjectiveFunction<StopTimeInfo>
    {
        private readonly Dictionary<int, int> _distancesToDestinationStop;
        private readonly Graph<int, StopTimeInfo> _graph;
        private readonly StopDto _referentialDestinationStop;
        private readonly List<GraphNode<int, StopTimeInfo>> _sourceNodes;
        private readonly Dictionary<int, int> _stopTimeIdToStopId;
        public Location Destination { get; }
        public Location Source { get; }

        public GraphObjectiveFunction(Graph<int, StopTimeInfo> graph, Location source, Location destination)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));

            _graph = graph ?? throw new ArgumentNullException(nameof(graph));

            // Set up source and destination nodes
            _referentialDestinationStop = GetReferenceDestinationStop();
            _sourceNodes = GetSourceNodes();
            _distancesToDestinationStop = GetDistancesToDestinationStop();
            _stopTimeIdToStopId = _graph.Nodes.Values.ToDictionary(k => k.Id, v => v.Data.StopId);
        }

        public double CalculateObjectiveValue(params StopTimeInfo[] arguments)
        {
            if (arguments.Last().StopDto.Name.TrimToLower() != Destination.Name.TrimToLower())
            {
                return double.PositiveInfinity;
            }

            var travelTime = (arguments.Last().DepartureTime - arguments.First().DepartureTime).TotalMinutes;
            var transferCount = arguments.Select(s => s.Route).Distinct().Count() - 1;

            return travelTime + transferCount;
        }

        public StopTimeInfo[] GetRandomArguments()
        {
            var sourceNode = GetRandomSourceNode();
            var randomPath = new List<StopTimeInfo>
            {
                sourceNode.Data
            };
            var currentNode = sourceNode;

            while (currentNode.Data.StopDto.Name.TrimToLower() != Destination.Name.TrimToLower())
            {
                var randomNeighborNode = GetRandomNeighborNodeCloserToDestination(currentNode, randomPath.ToArray());
                if (randomNeighborNode == null)
                {
                    break;
                }

                randomPath.Add(randomNeighborNode.Data);
                currentNode = randomNeighborNode;
            }

            return randomPath.ToArray();
        }

        public Harmony<StopTimeInfo> UsePitchAdjustment(Harmony<StopTimeInfo> harmony)
        {
            if (harmony.Arguments.Last().StopDto.Name.TrimToLower() == Destination.Name.TrimToLower())
            {
                return harmony;
            }

            var randomIndex = harmony.Arguments.GetRandomIndexMinimum(1);

            var predecessorStopTimeId = harmony.Arguments[randomIndex - 1].Id;
            var predecessorNode = _graph[predecessorStopTimeId];

            var pitchAdjustedSuccessor = GetRandomNeighborNodeExceptExisting(predecessorNode, harmony.Arguments);
            if (pitchAdjustedSuccessor != null)
            {
                harmony.Arguments[randomIndex] = pitchAdjustedSuccessor.Data;
            }

            harmony.ObjectiveValue = CalculateObjectiveValue(harmony.Arguments);

            return harmony;
        }

        private Dictionary<int, int> GetDistancesToDestinationStop()
        {
            var distances = _graph.Nodes
                .Select(n => n.Value.Data.StopDto)
                .Distinct()
                .ToDictionary(k => k.Id, k => k.GetDistanceTo(_referentialDestinationStop));

            return distances;
        }

        /// <summary>
        /// Gets a random neighbor of specified node
        /// </summary>
        /// <param name="currentNode">Node with neighbors</param>
        /// <returns>Random node neighbor</returns>
        private GraphNode<int, StopTimeInfo> GetRandomNeighborNode(GraphNode<int, StopTimeInfo> currentNode)
        {
            var neighbors = currentNode.Neighbors;

            if (!neighbors.Any())
            {
                return null;
            }

            var randomNeighborId = neighbors.GetRandomElement().DestinationId;

            return _graph[randomNeighborId];
        }

        /// <summary>
        /// Gets a random neighbor of specified node except if it already exists in harmony
        /// </summary>
        /// <param name="currentNode">Node with neighbors</param>
        /// <param name="harmonyArguments"></param>
        /// <returns>Random node neighbor</returns>
        private GraphNode<int, StopTimeInfo> GetRandomNeighborNodeCloserToDestination(GraphNode<int, StopTimeInfo> currentNode, StopTimeInfo[] harmonyArguments)
        {
            if (!currentNode.Neighbors.Any())
            {
                return null;
            }

            var distanceToDestination = _distancesToDestinationStop[currentNode.Data.StopId];

            var neighborStopIds = _graph.GetNeighborsQueryable(currentNode.Id)
                .Select(n => n.Data.StopId)
                .Distinct()
                .ToList();

            var stopsCloserToDestinationIds = neighborStopIds
                .Where(stopId => _distancesToDestinationStop[stopId] < distanceToDestination)
                .ToList();

            var randomStopId = stopsCloserToDestinationIds.GetRandomElement();

            if (randomStopId == default(int))
            {
                return null;
            }

            var forbiddenStopTimeIds = harmonyArguments.Select(a => a.Id).ToList();

            var firstStopTimeWithStop = _graph.GetNeighborsQueryable(currentNode.Id)
                .Where(s => s.Data.StopId == randomStopId && !forbiddenStopTimeIds.Contains(s.Data.Id))
                .OrderBy(s => s.Data.DepartureTime)
                .FirstOrDefault();

            return firstStopTimeWithStop;
        }

        private GraphNode<int, StopTimeInfo> GetRandomNeighborNodeExceptExisting(GraphNode<int, StopTimeInfo> currentNode, StopTimeInfo[] harmonyArguments)
        {
            var neighbors = currentNode.Neighbors;

            if (!neighbors.Any())
            {
                return null;
            }

            var forbiddenIds = harmonyArguments.Select(a => a.Id);
            var possibleNeighborIds = neighbors.Select(n => n.DestinationId).Except(forbiddenIds).ToArray();

            if (!possibleNeighborIds.Any())
            {
                return null;
            }

            var randomNeighborId = possibleNeighborIds.GetRandomElement();

            return _graph[randomNeighborId];
        }

        private GraphNode<int, StopTimeInfo> GetRandomSourceNode()
        {
            return _sourceNodes.GetRandomElement();
        }

        private StopDto GetReferenceDestinationStop()
        {
            return _graph.Nodes.Values.First(s => s.Data.StopDto.Name.TrimToLower() == Destination.Name.TrimToLower()).Data.StopDto;
        }

        private List<GraphNode<int, StopTimeInfo>> GetSourceNodes()
        {
            // Get source stops that have the same name
            var sourceStopTimesGroupedByStop = _graph.Nodes.Values
                .Where(s => s.Data.StopDto.Name.TrimToLower() == Source.Name.TrimToLower())
                .GroupBy(s => s.Data.StopDto)
                .ToDictionary(k => k.Key, g => g.GroupBy(st => st.Data.Route)
                    .Select(gr => gr.OrderBy(st => st.Data.DepartureTime).First().Id).ToList());

            // Calculate straight-line distance to destination
            var distanceToDestination = sourceStopTimesGroupedByStop.Keys
                .Select(s => s.GetDistanceTo(_referentialDestinationStop)).Max();

            var stopsWithRightDirectionIds = new List<int>();
            foreach (var stopWithTimes in sourceStopTimesGroupedByStop)
            {
                // Get neighbor stops
                var neighborStops = stopWithTimes.Value
                    .SelectMany(stopTimeInfoId => _graph.GetNeighborsQueryable(stopTimeInfoId)
                        .Select(n => n.Data.StopDto)
                        .Where(s => s.Name.TrimToLower() != Source.Name.TrimToLower()))
                    .Distinct()
                    .ToList();

                if (neighborStops.Any())
                {
                    var minimumDistanceToNeighbor = neighborStops
                        .Select(s => s.GetDistanceTo(_referentialDestinationStop))
                        .Min();

                    // Take only those stops which have neighbors closer to the destination
                    if (minimumDistanceToNeighbor < distanceToDestination)
                    {
                        stopsWithRightDirectionIds.Add(stopWithTimes.Key.Id);
                    }
                }
            }

            // Get matching graph nodes (get only one node per route id and stop id)
            var filteredSourceNodes = _graph.Nodes.Values
                .Where(s => stopsWithRightDirectionIds.Contains(s.Data.StopId))
                .GroupBy(s => s.Data.StopId)
                .SelectMany(g => g.GroupBy(st => st.Data.Route)
                    .Select(gr => gr.OrderBy(st => st.Data.DepartureTime).First()))
                .ToList();

            return filteredSourceNodes;
        }
    }
}