using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Utils;
using System.Collections.Generic;
using System.Linq;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    /// <summary>
    /// Objective function that randomly selects a stop from available neighbors
    /// </summary>
    public class RandomStopHarmonyGenerator : BaseStopTimeHarmonyGenerator
    {
        private readonly Dictionary<int, List<int>> _stopGraph;
        private readonly Dictionary<int, List<GraphNode<int, StopTimeInfo>>> _stopIdToStopTimes;

        public override HarmonyGeneratorType Type => HarmonyGeneratorType.RandomStop;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomStopHarmonyGenerator"/> class.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public RandomStopHarmonyGenerator(IObjectiveFunction<StopTimeInfo> function, Graph<int, StopTimeInfo> graph, Location source, Location destination) : base(function, graph, source, destination)
        {
            // Set up side graphs
            _stopIdToStopTimes = Graph.Nodes.Values
                .GroupBy(v => v.Data.StopId)
                .ToDictionary(k => k.Key, v => v.OrderBy(s => s.Data.DepartureTime).ToList());

            _stopGraph = Graph.Nodes.Values
                .GroupBy(s => s.Data.StopId)
                .ToDictionary(s => s.Key, v => v.SelectMany(s => s.Neighbors.Select(n => Graph[n.DestinationId].Data.StopId))
                    .Distinct()
                    .ToList());
        }

        /// <summary>
        /// Pitches the adjust harmony.
        /// </summary>
        /// <param name="harmony">The harmony.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the random arguments.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the random neighbor node except existing.
        /// </summary>
        /// <param name="currentNode">The current node.</param>
        /// <param name="harmonyArguments">The harmony arguments.</param>
        /// <returns></returns>
        private GraphNode<int, StopTimeInfo> GetRandomNeighborNodeExceptExisting(GraphNode<int, StopTimeInfo> currentNode, StopTimeInfo[] harmonyArguments)
        {
            var neighborStopIds = _stopGraph[currentNode.Data.StopId];

            var randomStopId = neighborStopIds.GetRandomElement();

            if (randomStopId == default)
            {
                return null;
            }

            var forbiddenStopTimeIds = harmonyArguments.Select(a => a.Id).ToList();

            var firstStopTimeWithStop = _stopIdToStopTimes[randomStopId]
                .FirstOrDefault(s => !forbiddenStopTimeIds.Contains(s.Data.Id)
                                     && currentNode.Neighbors.Select(n => n.DestinationId).Contains(s.Data.Id)
                                     && s.Data.DepartureTime > currentNode.Data.DepartureTime);

            return firstStopTimeWithStop;
        }
    }
}