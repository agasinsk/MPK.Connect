using MoreLinq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPK.Connect.Service.Business.HarmonySearch.Helpers
{
    /// <summary>
    /// Ant colony optimizer for stop time graph
    /// </summary>
    public class StopTimeAntColonyOptimizer : IAntColonyOptimizer<StopTimeInfo>
    {
        private readonly Graph<int, StopTimeInfo> _graph;
        private readonly IObjectiveFunction<StopTimeInfo> _objectiveFunction;
        private readonly double[] _pheromoneArray;
        private readonly Dictionary<int, Dictionary<int, int>> _pheromoneIndexes;
        private readonly StopDto _referentialDestinationStop;
        private readonly List<GraphNode<int, StopTimeInfo>> _sourceNodes;
        private readonly int RouteElementCountLimit = 200;
        public double EdgeCostInfluence { get; set; }
        public double InitialPheromoneAmount { get; set; }
        public double PheromoneEvaporationSpeed { get; set; }
        public double PheromoneInfluence { get; set; }
        public Location Source { get; }
        private Location Destination { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StopTimeAntColonyOptimizer"/> class.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
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

            // Initialize pheromone tables
            _pheromoneIndexes = _graph.Nodes.Values
                .ToDictionary(k => k.Id, k => k.Neighbors.ToDictionary(n => n.DestinationId, v => 0));
            _pheromoneArray = new double[_pheromoneIndexes.Sum(n => n.Value.Count)];

            var index = 0;

            foreach (var nodeId in _pheromoneIndexes.Keys.ToList())
            {
                var neighborNodesPheromone = _pheromoneIndexes[nodeId];

                foreach (var neighborId in neighborNodesPheromone.Keys.ToList())
                {
                    neighborNodesPheromone[neighborId] = index;
                    _pheromoneArray[index] = InitialPheromoneAmount;
                    index++;
                }
            }
        }

        /// <summary>
        /// Gets the ant colony solutions.
        /// </summary>
        /// <param name="solutionCount">The solution count.</param>
        /// <returns></returns>
        public IEnumerable<Harmony<StopTimeInfo>> GetAntColonySolutions(int solutionCount)
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
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            for (var i = 0; i < _pheromoneArray.Length; i++)
            {
                _pheromoneArray[i] = InitialPheromoneAmount;
            }
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
        /// Adds the pheromone amount.
        /// </summary>
        /// <param name="firstArgumentId">The first argument identifier.</param>
        /// <param name="nextArgumentId">The next argument identifier.</param>
        /// <param name="reinforcementAmount">The reinforcement amount.</param>
        private void AddPheromoneAmount(int firstArgumentId, int nextArgumentId, double reinforcementAmount)
        {
            if (_pheromoneIndexes[firstArgumentId].ContainsKey(nextArgumentId))
            {
                var pheromoneIndex = _pheromoneIndexes[firstArgumentId][nextArgumentId];

                _pheromoneArray[pheromoneIndex] += reinforcementAmount;
            }
        }

        /// <summary>
        /// Evaporates pheromone
        /// </summary>
        private void EvaporatePheromone()
        {
            for (var i = 0; i < _pheromoneArray.Length; i++)
            {
                _pheromoneArray[i] *= (1 - PheromoneEvaporationSpeed);
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

            while (currentNode.Data.StopDto.Name != _referentialDestinationStop.Name && arguments.Count < RouteElementCountLimit)
            {
                var nextNode = SelectNextNode(currentNode);

                if (nextNode == null)
                {
                    break;
                }

                arguments.Add(nextNode.Data);

                UpdateLocalPheromone(currentNode.Id, nextNode.Id);

                currentNode = nextNode;
            }

            var objectiveValue = _objectiveFunction.GetObjectiveValue(arguments.ToArray());

            return new Harmony<StopTimeInfo>(objectiveValue, arguments.ToArray());
        }

        /// <summary>
        /// Gets the node importance.
        /// </summary>
        /// <param name="currentNodeId">The current node identifier.</param>
        /// <param name="graphEdge">The graph edge.</param>
        /// <returns></returns>
        private double GetNodeImportance(int currentNodeId, GraphEdge<int> graphEdge)
        {
            var pheromoneAmount = GetPheromoneAmount(currentNodeId, graphEdge.DestinationId);

            var pheromone = Math.Pow(pheromoneAmount, PheromoneInfluence);
            var edgeCost = Math.Pow(graphEdge.Cost, EdgeCostInfluence);

            return pheromone * edgeCost;
        }

        /// <summary>
        /// Gets the pheromone amount.
        /// </summary>
        /// <param name="currentNodeId">The current node identifier.</param>
        /// <param name="destinationId">The destination identifier.</param>
        /// <returns></returns>
        private double GetPheromoneAmount(int currentNodeId, int destinationId)
        {
            var index = _pheromoneIndexes[currentNodeId][destinationId];

            return _pheromoneArray[index];
        }

        /// <summary>
        /// Gets the reinforcement coefficient.
        /// </summary>
        /// <param name="bestHarmony">The best harmony.</param>
        /// <returns></returns>
        private double GetReinforcementCoefficient(Harmony<StopTimeInfo> bestHarmony)
        {
            if (double.IsInfinity(bestHarmony.ObjectiveValue))
            {
                return double.NegativeInfinity;
            }

            return 1 / bestHarmony.ObjectiveValue;
        }

        /// <summary>
        /// Reinforces the pheromone on best harmony.
        /// </summary>
        /// <param name="bestHarmony">The best harmony.</param>
        private void ReinforcePheromoneOnBestHarmony(Harmony<StopTimeInfo> bestHarmony)
        {
            // Get reinforcement coefficient
            var reinforcementCoefficient = GetReinforcementCoefficient(bestHarmony);

            if (double.IsNegativeInfinity(reinforcementCoefficient))
            {
                return;
            }

            // Apply pheromone
            for (var index = 0; index < bestHarmony.Arguments.Length - 1; index++)
            {
                var firstArgumentId = bestHarmony.Arguments[index].Id;
                var nextArgumentId = bestHarmony.Arguments[index + 1].Id;

                var reinforcementAmount = PheromoneEvaporationSpeed * reinforcementCoefficient;

                AddPheromoneAmount(firstArgumentId, nextArgumentId, reinforcementAmount);
            }
        }

        /// <summary>
        /// Selects the next node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        private GraphNode<int, StopTimeInfo> SelectNextNode(GraphNode<int, StopTimeInfo> node)
        {
            if (!node.Neighbors.Any())
            {
                return null;
            }

            if (node.Neighbors.Count == 1)
            {
                return _graph[node.Neighbors.First().DestinationId];
            }

            var bestNeighborsIds = node.Neighbors
                .GroupBy(n => GetNodeImportance(node.Id, n), g => g.DestinationId)
                .OrderBy(g => g.Key)
                .Select(s => s.ToList())
                .FirstOrDefault();

            if (bestNeighborsIds == null)
            {
                return null;
            }

            // If there is more than 1 neighbor with the same fitness choose randomly
            var bestNeighborId = bestNeighborsIds.Count > 1 ?
                bestNeighborsIds.GetRandomElement() : bestNeighborsIds.First();

            return _graph[bestNeighborId];
        }

        /// <summary>
        /// Sets the pheromone amount.
        /// </summary>
        /// <param name="firstArgumentId">The first argument identifier.</param>
        /// <param name="nextArgumentId">The next argument identifier.</param>
        /// <param name="amount">The new amount.</param>
        private void SetPheromoneAmount(int firstArgumentId, int nextArgumentId, double amount)
        {
            var pheromoneIndex = _pheromoneIndexes[firstArgumentId][nextArgumentId];
            _pheromoneArray[pheromoneIndex] += amount;
        }

        /// <summary>
        /// Updates the pheromone locally.
        /// </summary>
        /// <param name="currentNodeId">The current node identifier.</param>
        /// <param name="nextNodeId">The next node identifier.</param>
        private void UpdateLocalPheromone(int currentNodeId, int nextNodeId)
        {
            var pheromoneAmount = GetPheromoneAmount(currentNodeId, nextNodeId);

            var pheromoneAmountAfterEvaporation = pheromoneAmount * (1 - PheromoneEvaporationSpeed);
            var addedPheromoneAmount = PheromoneEvaporationSpeed * 1 / InitialPheromoneAmount;
            var newAmount = pheromoneAmountAfterEvaporation + addedPheromoneAmount;

            SetPheromoneAmount(currentNodeId, nextNodeId, newAmount);
        }
    }
}