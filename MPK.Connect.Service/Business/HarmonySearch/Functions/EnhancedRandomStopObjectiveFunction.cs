using System;
using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Business.HarmonySearch.Functions
{
    public class EnhancedRandomStopObjectiveFunction : IGeneralObjectiveFunction<StopTimeInfo>
    {
        private readonly Graph<int, StopTimeInfo> _graph;
        private readonly StopDto _referentialDestinationStop;
        private readonly List<GraphNode<int, StopTimeInfo>> _sourceNodes;
        private readonly Dictionary<int, List<int>> _stopGraph;
        private readonly Dictionary<int, List<GraphNode<int, StopTimeInfo>>> _stopIdToStopTimes;
        public Location Destination { get; }
        public Location Source { get; }

        public ObjectiveFunctionType Type => ObjectiveFunctionType.EnhancedRandomStop;

        public EnhancedRandomStopObjectiveFunction(Graph<int, StopTimeInfo> graph, Location source, Location destination)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));

            _graph = graph ?? throw new ArgumentNullException(nameof(graph));

            // Set up source and destination nodes
            _referentialDestinationStop = GetReferenceDestinationStop();
            _sourceNodes = GetSourceNodes();
            _stopIdToStopTimes = _graph.Nodes.Values
                .GroupBy(v => v.Data.StopId)
                .ToDictionary(k => k.Key, v => v.OrderBy(s => s.Data.DepartureTime).ToList());

            _stopGraph = _graph.Nodes.Values
                .GroupBy(s => s.Data.StopId)
                .ToDictionary(s => s.Key, v => v.SelectMany(s => s.Neighbors.Select(n => _graph[n.DestinationId].Data.StopId))
                    .Distinct()
                    .ToList());

            //var stopGraph = _graph.Nodes.Values
            //    .GroupBy(s => s.Data.StopId)
            //    .ToDictionary(s => s.Key, v => v.SelectMany(s => s.Neighbors
            //        .Select(n => _graph[n.DestinationId].Data))
            //        .GroupBy(d => d.StopId)
            //        .ToDictionary(x => x.Key, y => y.ToList()));
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
                var randomNeighborNodes = GetRandomNeighborNodeExceptExisting(currentNode, randomPath.ToArray());
                if (randomNeighborNodes == null)
                {
                    break;
                }

                randomPath.AddRange(randomNeighborNodes.Select(n => n.Data));
                currentNode = randomNeighborNodes.Last();
            }

            return randomPath.ToArray();
        }

        public Harmony<StopTimeInfo> UsePitchAdjustment(Harmony<StopTimeInfo> harmony)
        {
            if (harmony.Arguments.Length < 2 || harmony.Arguments.Last().StopDto.Name.TrimToLower() == Destination.Name.TrimToLower())
            {
                return harmony;
            }

            var randomIndex = harmony.Arguments.GetRandomIndexMinimum(1);

            var predecessorStopTimeId = harmony.Arguments[randomIndex - 1].Id;
            var predecessorNode = _graph[predecessorStopTimeId];

            var pitchAdjustedSuccessor = GetRandomNeighborNodeExceptExisting(predecessorNode, harmony.Arguments);
            if (pitchAdjustedSuccessor != null)
            {
                harmony.Arguments[randomIndex] = pitchAdjustedSuccessor.Last().Data;
            }

            harmony.ObjectiveValue = CalculateObjectiveValue(harmony.Arguments);

            return harmony;
        }

        private List<GraphNode<int, StopTimeInfo>> GetRandomNeighborNodeExceptExisting(GraphNode<int, StopTimeInfo> currentNode, StopTimeInfo[] harmonyArguments)
        {
            var neighborStopIds = _stopGraph[currentNode.Data.StopId];

            var randomStopId = neighborStopIds.GetRandomElement();

            if (randomStopId == default(int))
            {
                return null;
            }

            var forbiddenStopTimeIds = harmonyArguments.Select(a => a.Id).ToList();

            var availableStopTimes = _stopIdToStopTimes[randomStopId]
                .Where(s => !forbiddenStopTimeIds.Contains(s.Data.Id))
                .ToList();

            var firstStopTimeWithStop = availableStopTimes
                .FirstOrDefault(s => s.Data.DepartureTime > currentNode.Data.DepartureTime);

            if (firstStopTimeWithStop == null)
            {
                return null;
            }

            if (IsAdditionalTransferStopTimeRequired(currentNode.Data, firstStopTimeWithStop.Data))
            {
                var matchingStop = _graph.GetNeighborsQueryable(currentNode.Id)
                    .FirstOrDefault(s => s.Data.StopDto.Name == firstStopTimeWithStop.Data.StopDto.Name);

                if (matchingStop == null)
                {
                    matchingStop = _graph.GetNeighborsQueryable(firstStopTimeWithStop.Id)
                        .FirstOrDefault(s => s.Data.StopDto.Name == currentNode.Data.StopDto.Name);
                }

                var stops = new List<GraphNode<int, StopTimeInfo>> { matchingStop,
                 firstStopTimeWithStop };

                return stops;
            }

            return new List<GraphNode<int, StopTimeInfo>> { firstStopTimeWithStop };
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

        /// <summary>
        /// Determines if additional stop time should be inserted to make the path feasible
        /// </summary>
        /// <param name="firstStop">First stop</param>
        /// <param name="secondStop">Second stop</param>
        /// <returns></returns>
        private bool IsAdditionalTransferStopTimeRequired(StopTimeInfo firstStop, StopTimeInfo secondStop)
        {
            // Stop name has to be different
            var stopNamesAreDifferent = firstStop.StopDto.Name != secondStop.StopDto.Name;

            if (stopNamesAreDifferent)
            {
                // Route and direction has to be different
                return firstStop.Route != secondStop.Route
                       && firstStop.DirectionId != secondStop.DirectionId;
            }

            return false;
        }
    }
}