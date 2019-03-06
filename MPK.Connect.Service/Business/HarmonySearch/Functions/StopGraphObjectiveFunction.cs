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
    public class StopGraphObjectiveFunction : IGeneralObjectiveFunction<StopDto>
    {
        private readonly Graph<int, StopDto> _graph;
        private readonly StopDto _referentialDestinationStop;
        private readonly List<GraphNode<int, StopDto>> _sourceNodes;
        public Location Destination { get; }
        public Location Source { get; }

        public StopGraphObjectiveFunction(Graph<int, StopDto> graph, Location source, Location destination)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));

            _graph = graph ?? throw new ArgumentNullException(nameof(graph));

            // Set up source and destination nodes
            _referentialDestinationStop = GetReferenceDestinationStop();
            _sourceNodes = GetSourceNodes();
        }

        public double CalculateObjectiveValue(params StopDto[] arguments)
        {
            if (arguments.Last().Name.TrimToLower() != Destination.Name.TrimToLower())
            {
                return double.MaxValue;
            }

            var graphEdges = _graph.GetEdges();

            var travelTime = 0d;
            for (var index = 0; index < arguments.Length - 1; index++)
            {
                var currentStop = arguments[index];
                var nextStop = arguments[index + 1];
                var edgeCost = graphEdges[currentStop.Id][nextStop.Id];
                travelTime += edgeCost;
            }

            return travelTime;
        }

        public StopDto[] GetRandomArguments()
        {
            var sourceNode = GetRandomSourceNode();
            var randomPath = new List<StopDto>
            {
                sourceNode.Data
            };
            var currentNode = sourceNode;

            while (currentNode.Data.Name != Destination.Name)
            {
                var randomNeighborNode = GetRandomNeighborNodeExceptExisting(currentNode, randomPath.ToArray());
                if (randomNeighborNode == null)
                {
                    break;
                }

                randomPath.Add(randomNeighborNode.Data);
                currentNode = randomNeighborNode;
            }

            return randomPath.ToArray();
        }

        public Harmony<StopDto> UsePitchAdjustment(Harmony<StopDto> harmony)
        {
            var randomArgumentWithIndex = harmony.Arguments.Skip(1).ToArray().GetRandomElementWithIndex();

            var graphNode = _graph[randomArgumentWithIndex.Value.Id];

            var pitchAdjustedValue = GetRandomNeighborNodeExceptExisting(graphNode, harmony.Arguments);
            if (pitchAdjustedValue != null)
            {
                harmony.Arguments[randomArgumentWithIndex.Key + 1] = pitchAdjustedValue.Data;
            }

            return harmony;
        }

        private Dictionary<int, int> GetDistancesToDestinationStop()
        {
            var distances = _graph.Nodes
                .Select(n => n.Value.Data)
                .Distinct()
                .ToDictionary(k => k.Id, k => k.GetDistanceTo(_referentialDestinationStop));

            return distances;
        }

        /// <summary>
        /// Gets a random neighbor of specified node
        /// </summary>
        /// <param name="currentNode">Node with neighbors</param>
        /// <returns>Random node neighbor</returns>
        private GraphNode<int, StopDto> GetRandomNeighborNode(GraphNode<int, StopDto> currentNode)
        {
            var neighbors = currentNode.Neighbors;

            if (!neighbors.Any())
            {
                return null;
            }

            var randomNeighborId = neighbors.GetRandomElement().DestinationId;

            return _graph[randomNeighborId];
        }

        private GraphNode<int, StopDto> GetRandomNeighborNodeExceptExisting(GraphNode<int, StopDto> currentNode,
            StopDto[] harmonyArguments)
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

        private GraphNode<int, StopDto> GetRandomSourceNode()
        {
            return _sourceNodes.GetRandomElement();
        }

        private StopDto GetReferenceDestinationStop()
        {
            return _graph.Nodes.Values.First(s => s.Data.Name.TrimToLower() == Destination.Name.TrimToLower()).Data;
        }

        private List<GraphNode<int, StopDto>> GetSourceNodes()
        {
            // Get source stops that have the same name
            return _graph.Nodes.Values
                .Where(s => s.Data.Name.TrimToLower() == Source.Name.TrimToLower())
                .ToList();
        }
    }
}