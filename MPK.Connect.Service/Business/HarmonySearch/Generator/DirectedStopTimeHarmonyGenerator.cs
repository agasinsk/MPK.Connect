using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    /// <summary>
    /// Harmony generator that takes the stop direction into account
    /// </summary>
    public class DirectedStopTimeHarmonyGenerator : BaseStopTimeHarmonyGenerator
    {
        private readonly Dictionary<int, int> _distancesToDestinationStop;
        private readonly Dictionary<int, List<int>> _stopGraph;
        private readonly Dictionary<int, List<GraphNode<int, StopTimeInfo>>> _stopIdToStopTimes;

        public override HarmonyGeneratorType Type => HarmonyGeneratorType.RandomStopDirected;

        public DirectedStopTimeHarmonyGenerator(IObjectiveFunction<StopTimeInfo> function, Graph<int, StopTimeInfo> graph, Location source, Location destination) : base(function, graph, source, destination)
        {
            // Set up side graphs and lookup tables
            _distancesToDestinationStop = GetDistancesToDestinationStop();

            _stopIdToStopTimes = Graph.Nodes.Values
                .GroupBy(v => v.Data.StopId)
                .ToDictionary(k => k.Key, v => v.OrderBy(s => s.Data.DepartureTime).ToList());

            _stopGraph = Graph.Nodes.Values
                .GroupBy(s => s.Data.StopId)
                .ToDictionary(s => s.Key, v => v.SelectMany(s => s.Neighbors.Select(n => Graph[n.DestinationId].Data.StopId))
                    .Distinct()
                    .ToList());
        }

        public override Harmony<StopTimeInfo> PitchAdjustHarmony(Harmony<StopTimeInfo> harmony)
        {
            if (harmony.Arguments.Length < 2 || harmony.Arguments.Last().StopDto.Name.TrimToLower() == Destination.Name.TrimToLower())
            {
                return harmony;
            }

            var randomIndex = harmony.Arguments.GetRandomIndexMinimum(1);

            var predecessorStopTimeId = harmony.Arguments[randomIndex - 1].Id;
            var predecessorNode = Graph[predecessorStopTimeId];

            var pitchAdjustedSuccessor = GetRandomNeighborNodeExceptExisting(predecessorNode, harmony.Arguments);
            if (pitchAdjustedSuccessor != null)
            {
                harmony.Arguments[randomIndex] = pitchAdjustedSuccessor.Data;
            }

            harmony.ObjectiveValue = ObjectiveFunction.GetObjectiveValue(harmony.Arguments);

            return harmony;
        }

        protected override StopTimeInfo[] GetRandomArguments()
        {
            var sourceNode = SourceNodes.GetRandomElement();
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

        private Dictionary<int, int> GetDistancesToDestinationStop()
        {
            var distances = Graph.Nodes
                .Select(n => n.Value.Data.StopDto)
                .Distinct()
                .ToDictionary(k => k.Id, k => k.GetDistanceTo(ReferentialDestinationStop));

            return distances;
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

            var neighborStopIds = _stopGraph[currentNode.Data.StopId];

            var neighborStopsCloserToDestinationIds = neighborStopIds
                .Where(stopId => _distancesToDestinationStop[stopId] < distanceToDestination)
                .ToList();

            var randomStopId = neighborStopsCloserToDestinationIds.GetRandomElement();

            if (randomStopId == default)
            {
                return null;
            }

            var forbiddenStopTimeIds = harmonyArguments.Select(a => a.Id).ToList();

            var firstStopTimeWithStop = _stopIdToStopTimes[randomStopId]
                .FirstOrDefault(s => !forbiddenStopTimeIds.Contains(s.Data.Id)
                                     && s.Data.DepartureTime > currentNode.Data.DepartureTime);

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

            return Graph[randomNeighborId];
        }
    }
}