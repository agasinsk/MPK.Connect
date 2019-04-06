using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Business.HarmonySearch.Helpers
{
    /// <summary>
    /// Ant colony optimizer for stop time graph
    /// </summary>
    public class StopTimeAntColonyOptimizer : IAntColonyOptimizer<StopTimeInfo>
    {
        private readonly Graph<int, StopTimeInfo> _graph;
        private readonly IObjectiveFunction<StopTimeInfo> _objectiveFunction;
        private readonly Dictionary<int, Dictionary<int, double>> _pheromoneAmounts;
        private readonly StopDto _referentialDestinationStop;
        private readonly List<GraphNode<int, StopTimeInfo>> _sourceNodes;

        public double EdgeCostInfluence { get; set; }
        public double InitialPheromoneAmount { get; set; }
        public double PheromoneEvaporationSpeed { get; set; }
        public double PheromoneInfluence { get; set; }
        public Location Source { get; }
        private Location Destination { get; }

        public StopTimeAntColonyOptimizer(IObjectiveFunction<StopTimeInfo> function, Graph<int, StopTimeInfo> graph, Location source, Location destination)
        {
            _objectiveFunction = function ?? throw new ArgumentNullException(nameof(function));
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));
            Source = source ?? throw new ArgumentNullException(nameof(source));

            // Set up source and destination nodes
            _referentialDestinationStop = graph.GetReferenceDestinationStop(Destination.Name);
            _sourceNodes = graph.GetSourceNodes(Source.Name, _referentialDestinationStop);

            // Set up ant colony specifics
            PheromoneInfluence = 0.2;
            EdgeCostInfluence = 1 - PheromoneInfluence;

            InitialPheromoneAmount = 1;

            PheromoneEvaporationSpeed = 0.05;

            _pheromoneAmounts = _graph.Nodes.Values
                .ToDictionary(k => k.Id, k => k.Neighbors.ToDictionary(v => v.DestinationId, v => InitialPheromoneAmount));
        }

        public IEnumerable<Harmony<StopTimeInfo>> GetAntSolutions(int solutionCount)
        {
            var antSolutions = new List<Harmony<StopTimeInfo>>(solutionCount);

            for (var i = 0; i < solutionCount; i++)
            {
                var antSolution = GetAntSolution();
                antSolutions.Add(antSolution);
            }

            return antSolutions;
        }

        /// <summary>
        /// Updates global pheromone levels
        /// </summary>
        /// <param name="bestHarmony">Best harmony</param>
        public void UpdateGlobalPheromone(Harmony<StopTimeInfo> bestHarmony)
        {
            EvaporatePheromone();

            ReinforcePheromoneOnBestHarmony(bestHarmony);
        }

        /// <summary>
        /// Evaporates pheromone
        /// </summary>
        private void EvaporatePheromone()
        {
            var pheromoneAmounts = _pheromoneAmounts.SelectMany(k => k.Value.Select(v => v.Value)).ToList();

            for (var index = 0; index < pheromoneAmounts.Count; index++)
            {
                var pheromoneAmount = pheromoneAmounts[index];
                var pheromoneAmountAfterEvaporation = pheromoneAmount * (1 - PheromoneEvaporationSpeed);

                pheromoneAmounts[index] = pheromoneAmountAfterEvaporation;
            }
        }

        /// <summary>
        /// Gets single ant solution
        /// </summary>
        /// <returns></returns>
        private Harmony<StopTimeInfo> GetAntSolution()
        {
            var currentNode = _sourceNodes.GetRandomElement();

            var arguments = new List<StopTimeInfo> { currentNode.Data };

            while (currentNode != null && currentNode.Data.StopDto.Name != _referentialDestinationStop.Name)
            {
                var nextNode = SelectNextNode(currentNode);

                arguments.Add(nextNode.Data);

                UpdateLocalPheromone(currentNode.Id, nextNode.Id);

                currentNode = nextNode;
            }

            var objectiveValue = _objectiveFunction.GetObjectiveValue(arguments.ToArray());

            return new Harmony<StopTimeInfo>(objectiveValue, arguments.ToArray());
        }

        private double GetNodeImportance(int currentNodeId, GraphEdge<int> graphEdge)
        {
            var pheromone = Math.Pow(_pheromoneAmounts[currentNodeId][graphEdge.DestinationId], PheromoneInfluence);
            var edgeCost = Math.Pow(graphEdge.Cost, EdgeCostInfluence);

            return pheromone * edgeCost;
        }

        private double GetReinforcementCoefficient(Harmony<StopTimeInfo> bestHarmony)
        {
            if (double.IsInfinity(bestHarmony.ObjectiveValue))
            {
                return 0d;
            }

            return 1 / bestHarmony.ObjectiveValue;
        }

        private void ReinforcePheromoneOnBestHarmony(Harmony<StopTimeInfo> bestHarmony)
        {
            var reinforcementCoefficient = GetReinforcementCoefficient(bestHarmony);
            for (var index = 0; index < bestHarmony.Arguments.Length - 1; index++)
            {
                var firstArgumentId = bestHarmony.Arguments[index].Id;
                var nextArgumentId = bestHarmony.Arguments[index + 1].Id;

                var reinforcementAmount = _pheromoneAmounts[firstArgumentId][nextArgumentId] + PheromoneEvaporationSpeed * reinforcementCoefficient;

                _pheromoneAmounts[firstArgumentId][nextArgumentId] += reinforcementAmount;
            }
        }

        private GraphNode<int, StopTimeInfo> SelectNextNode(GraphNode<int, StopTimeInfo> node)
        {
            if (!node.Neighbors.Any())
            {
                return null;
            }

            var bestNeighborId = node.Neighbors
                .ToDictionary(n => n.DestinationId, n => GetNodeImportance(node.Id, n))
                .OrderBy(n => n.Value)
                .FirstOrDefault().Key;

            return _graph[bestNeighborId];
        }

        private void UpdateLocalPheromone(int currentNodeId, int nextNodeId)
        {
            var pheromoneAmount = _pheromoneAmounts[currentNodeId][nextNodeId];
            var pheromoneAmountAfterEvaporation = pheromoneAmount * (1 - PheromoneEvaporationSpeed);

            var addedPheromoneAmount = PheromoneEvaporationSpeed * 1 / InitialPheromoneAmount;

            _pheromoneAmounts[currentNodeId][nextNodeId] = pheromoneAmountAfterEvaporation + addedPheromoneAmount;
        }
    }
}