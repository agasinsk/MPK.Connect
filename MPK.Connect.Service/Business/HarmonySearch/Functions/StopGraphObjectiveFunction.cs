using System;
using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Helpers;
using MPK.Connect.Service.Utils;

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
                return double.PositiveInfinity;
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
            // TODO: rethink strategy
            var randomIndex = harmony.Arguments.GetRandomIndexMinimum(1);

            // Get the predecessor of randomly selected node
            var predecessorIndex = harmony.Arguments[randomIndex].Id;
            var predecessorNode = _graph[predecessorIndex];

            var pitchAdjustedValue = GetRandomNeighborNodeExceptExisting(predecessorNode, harmony.Arguments);
            if (pitchAdjustedValue != null)
            {
                harmony.Arguments[randomIndex] = pitchAdjustedValue.Data;
            }

            harmony.ObjectiveValue = CalculateObjectiveValue(harmony.Arguments);

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