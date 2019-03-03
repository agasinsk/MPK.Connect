﻿using System;
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
        private readonly Location _destination;
        private readonly Graph<int, StopTimeInfo> _graph;
        private readonly StopDto _referentialDestinationStop;
        private readonly Location _source;
        private readonly List<GraphNode<int, StopTimeInfo>> _sourceNodes;

        public GraphObjectiveFunction(Graph<int, StopTimeInfo> graph, Location source, Location destination)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _destination = destination ?? throw new ArgumentNullException(nameof(destination));

            _graph = graph ?? throw new ArgumentNullException(nameof(graph));

            // Set up source and destination nodes
            _referentialDestinationStop = GetReferenceDestinationStop();
            _sourceNodes = GetSourceNodes();
        }

        public double CalculateObjectiveValue(params StopTimeInfo[] arguments)
        {
            var travelTime = (arguments.Last().DepartureTime - arguments.First().DepartureTime).TotalMinutes;
            var transferCount = arguments.Select(s => s.Route).Distinct().Count();

            var additionalPenalty = 0;
            if (arguments.Last().StopDto.Name.TrimToLower() != _destination.Name)
            {
                // TODO: define cost function with penalties(!)
                additionalPenalty = int.MaxValue;
            }

            return travelTime + 10 * transferCount + additionalPenalty;
        }

        public StopTimeInfo[] GetRandomArguments()
        {
            var sourceNode = GetRandomSourceNode();
            var randomPath = new List<StopTimeInfo>
            {
                sourceNode.Data
            };
            var currentNode = sourceNode;

            while (currentNode.Data.StopDto.Name != _destination.Name)
            {
                var randomNeighborNode = GetRandomNeighborNode(currentNode);
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
            var randomArgumentWithIndex = harmony.Arguments.GetRandomElementWithIndex();

            var graphNode = _graph[randomArgumentWithIndex.Value.Id];

            var pitchAdjustedValue = GetRandomNeighborNodeCloserToDestination(graphNode);
            if (pitchAdjustedValue != null)
            {
                harmony.Arguments[randomArgumentWithIndex.Key] = pitchAdjustedValue.Data;
            }

            return harmony;
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
        /// Gets a random neighbor of specified node
        /// </summary>
        /// <param name="currentNode">Node with neighbors</param>
        /// <returns>Random node neighbor</returns>
        private GraphNode<int, StopTimeInfo> GetRandomNeighborNodeCloserToDestination(GraphNode<int, StopTimeInfo> currentNode)
        {
            if (!currentNode.Neighbors.Any())
            {
                return null;
            }

            var distanceToDestination = currentNode.Data.StopDto.GetDistanceTo(_referentialDestinationStop);

            var distances = _graph.GetNeighborsQueryable(currentNode.Id)
                .GroupBy(s => s.Data.StopDto)
                .ToDictionary(n => n.Key, n => n.Key.GetDistanceTo(_referentialDestinationStop));

            var neighborStopWithNodeIds = _graph.GetNeighborsQueryable(currentNode.Id)
                .GroupBy(s => s.Data.StopDto)
                .Where(n => n.Key.GetDistanceTo(_referentialDestinationStop) < distanceToDestination)
                .ToDictionary(k => k.Key, g => g.GroupBy(st => st.Data.Route)
                    .Select(gr => gr.OrderBy(st => st.Data.DepartureTime).First().Id));

            var neighborsCloserToDestinationIds = neighborStopWithNodeIds
                .SelectMany(x => x.Value)
                .ToList();

            var randomNeighborId = neighborsCloserToDestinationIds.GetRandomElement();

            if (randomNeighborId == default(int))
            {
                return null;
            }

            var randomNeighbor = _graph[randomNeighborId];

            return randomNeighbor;
        }

        private GraphNode<int, StopTimeInfo> GetRandomSourceNode()
        {
            return _sourceNodes.GetRandomElement();
        }

        private StopDto GetReferenceDestinationStop()
        {
            return _graph.Nodes.Values.First(s => s.Data.StopDto.Name.TrimToLower() == _destination.Name.TrimToLower()).Data.StopDto;
        }

        private List<GraphNode<int, StopTimeInfo>> GetSourceNodes()
        {
            // Get source stops that have the same name
            var sourceStopTimesGroupedByStop = _graph.Nodes.Values
                .Where(s => s.Data.StopDto.Name.TrimToLower() == _source.Name.TrimToLower())
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
                        .Where(s => s.Name.TrimToLower() != _source.Name.TrimToLower()))
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
        /// Gets maximum distance to destination
        /// </summary>
        /// <param name="stops"></param>
        /// <returns></returns>
        private double GetStraightLineDistanceToDestination(IEnumerable<StopDto> stops)
        {
            return stops.Select(s => s.GetDistanceTo(_referentialDestinationStop)).Max();
        }
    }
}