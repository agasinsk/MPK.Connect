using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Business.HarmonySearch.Functions
{
    /// <summary>
    /// Objective function that randomly selects stop time from available neighbors
    /// </summary>
    public class AntColonyObjectiveFunction : BaseStopTimeObjectiveFunction, IAntColonyObjectiveFunction<StopTimeInfo>
    {
        private readonly Dictionary<int, Dictionary<int, double>> _pheromoneAmounts;
        public int AntCount { get; set; }

        public double EdgeCostInfluence { get; set; }
        public int InitialPheromoneAmount { get; set; }
        public double PheromoneEvaporationSpeed { get; set; }
        public double PheromoneInfluence { get; set; }

        public override ObjectiveFunctionType Type => ObjectiveFunctionType.AntColony;

        public AntColonyObjectiveFunction(Graph<int, StopTimeInfo> graph, Location source, Location destination) : base(graph, source, destination)
        {
            PheromoneInfluence = 0.2;
            EdgeCostInfluence = 1 - PheromoneInfluence;

            InitialPheromoneAmount = 1;

            PheromoneEvaporationSpeed = 0.5;
            AntCount = graph.Select(s => s.StopDto).Distinct().Count();
        }

        public override double CalculateObjectiveValue(params StopTimeInfo[] arguments)
        {
            if (arguments.Last().StopDto.Name.TrimToLower() != Destination.Name.TrimToLower())
            {
                return double.PositiveInfinity;
            }

            var travelTime = (arguments.Last().DepartureTime - arguments.First().DepartureTime).TotalMinutes;
            var transferCount = arguments.Select(s => s.Route).Distinct().Count() - 1;

            return travelTime + transferCount;
        }

        public IEnumerable<Harmony<StopTimeInfo>> GetAntSolutions()
        {
            throw new System.NotImplementedException();
        }

        public override StopTimeInfo[] GetRandomArguments()
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

        public override Harmony<StopTimeInfo> UsePitchAdjustment(Harmony<StopTimeInfo> harmony)
        {
            if (harmony.Arguments.Last().StopDto.Name.TrimToLower() == Destination.Name.TrimToLower())
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

            harmony.ObjectiveValue = CalculateObjectiveValue(harmony.Arguments);

            return harmony;
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