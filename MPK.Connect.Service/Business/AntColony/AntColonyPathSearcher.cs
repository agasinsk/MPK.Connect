using System;
using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Business.AntColony
{
    public class AntColonyPathSearcher
    {
        private readonly List<Ant> _ants;
        private readonly Graph<int, StopTimeInfo> _graph;
        private readonly IBoundedRandom _random;
        private readonly StopDto _referentialDestinationStop;
        private readonly List<GraphNode<int, StopTimeInfo>> _sourceNodes;
        private readonly Dictionary<int, Dictionary<int, bool>> _visitedEdges;
        private readonly Dictionary<int, bool> _visitedNodes;
        private Dictionary<int, Dictionary<int, double>> _pheromoneAmounts;
        public int AntCount { get; set; }

        public Location Destination { get; }

        public double EdgeCostInfluence { get; set; }

        public double InitialPheromoneAmount { get; set; }
        public int MaxIterationCount { get; set; }
        public double MaxPheromoneAmount { get; set; }
        public double MinPheromoneAmount { get; set; }
        public double PheromoneAmountPerAnt { get; set; }
        public double PheromoneEvaporationSpeed { get; set; }

        public double PheromoneInfluence { get; set; }

        public Location Source { get; }

        public AntColonyPathSearcher(Graph<int, StopTimeInfo> graph, Location source, Location destination)
        {
            PheromoneInfluence = 0.2;
            EdgeCostInfluence = 1 - PheromoneInfluence;
            MaxIterationCount = 10000;
            InitialPheromoneAmount = 1;
            PheromoneAmountPerAnt = InitialPheromoneAmount;
            MinPheromoneAmount = InitialPheromoneAmount;
            PheromoneEvaporationSpeed = 0.5;
            AntCount = graph.Select(s => s.StopDto).Distinct().Count();

            Source = source ?? throw new ArgumentNullException(nameof(source));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));

            _graph = graph ?? throw new ArgumentNullException(nameof(graph));

            _random = RandomFactory.GetInstance();

            // Set up source and destination nodes
            _referentialDestinationStop = GetReferenceDestinationStop();
            _sourceNodes = GetSourceNodes();

            //// Initialize visited nodes
            //_visitedNodes = _graph.Nodes.Values
            //    .ToDictionary(k => k.Id, k => false);

            //// Initialize visited edges
            //_visitedEdges = _graph.Nodes.Values
            //    .ToDictionary(k => k.Id, k => k.Neighbors.ToDictionary(v => v.DestinationId, v => false));

            // Initialize pheromone amount lookup
            _pheromoneAmounts = _graph.Nodes.Values
                .ToDictionary(k => k.Id, k => k.Neighbors.ToDictionary(v => v.DestinationId, v => InitialPheromoneAmount));

            // Initialize ants
            _ants = Enumerable.Range(1, AntCount)
                .Select(i => new Ant(i))
                .ToList();

            // Set source node for each ant
            foreach (var ant in _ants)
            {
                var sourceNodeId = _sourceNodes.GetRandomElement().Id;

                ant.VisitNode(sourceNodeId);
            }
        }

        public Path<StopTimeInfo> SearchForPath()
        {
            var iterationCount = 0;
            while (iterationCount < MaxIterationCount)
            {
                MoveAnts();
                UpdatePheromoneTrails();
                iterationCount++;
            }

            return GetBestPath();
        }

        private void AddAntPheromones()
        {
            foreach (var ant in _ants)
            {
                for (int i = 0; i < ant.VisitedNodeIds.Count - 1; i++)
                {
                    var firstNodeId = ant.VisitedNodeIds[i];
                    var nextNodeId = ant.VisitedNodeIds[i + 1];

                    _pheromoneAmounts[firstNodeId][nextNodeId] += PheromoneAmountPerAnt;
                }
            }
        }

        private void ClearPheromoneTrails()
        {
            _pheromoneAmounts = _graph.Nodes.Values
                .ToDictionary(k => k.Id, k => k.Neighbors.ToDictionary(v => v.DestinationId, v => InitialPheromoneAmount));
        }

        private void EvaporatePheromone()
        {
            var pheromoneAmounts = _pheromoneAmounts.SelectMany(k => k.Value.Select(v => v.Value)).ToList();
            for (var index = 0; index < pheromoneAmounts.Count; index++)
            {
                var pheromoneAmount = pheromoneAmounts[index];
                var pheromoneAmountAfterEvaporation = pheromoneAmount * (1 - PheromoneEvaporationSpeed);
                pheromoneAmounts[index] = pheromoneAmountAfterEvaporation < MinPheromoneAmount
                    ? MinPheromoneAmount
                    : pheromoneAmountAfterEvaporation;
            }
        }

        private Path<StopTimeInfo> GetBestPath()
        {
            var feasibleAnts = _ants.Where(a =>
                    _graph[a.VisitedNodeIds.Last()].Data.StopDto.Name.TrimToLower() == Destination.Name.TrimToLower())
                .ToList();

            var antWithBestPath = _ants.OrderBy(a => a.PathCost).First();

            var antVisitedNodesIds = antWithBestPath.VisitedNodeIds;

            var graphNodes = _graph.Nodes.Values
                .Where(n => antVisitedNodesIds.Contains(n.Id))
                .Select(n => n.Data)
                .ToList();

            return new Path<StopTimeInfo>(graphNodes, antWithBestPath.PathCost);
        }

        private double GetEdgeCoefficient(int nodeId, GraphEdge<int> edge)
        {
            var pheromoneAmount = _pheromoneAmounts[nodeId][edge.DestinationId];
            if (pheromoneAmount.AlmostEquals(0))
            {
                return Math.Pow(1 / GetEdgeImportance(edge, pheromoneAmount), PheromoneInfluence);
            }

            return GetEdgeImportance(edge, pheromoneAmount);
        }

        private double GetEdgeImportance(GraphEdge<int> edge, double pheromoneAmount)
        {
            return pheromoneAmount * PheromoneInfluence + (1 - PheromoneInfluence) * edge.Cost;
        }

        private StopDto GetReferenceDestinationStop()
        {
            return _graph.Nodes.Values
                .First(s => s.Data.StopDto.Name.TrimToLower() == Destination.Name.TrimToLower())
                .Data.StopDto;
        }

        private List<GraphNode<int, StopTimeInfo>> GetSourceNodes()
        {
            // Get source stops that have the same name
            var sourceStopTimesGroupedByStop = _graph.Nodes.Values
                .Where(s => s.Data.StopDto.Name.TrimToLower() == Source.Name.TrimToLower())
                .GroupBy(s => s.Data.StopDto)
                .ToDictionary(k => k.Key, v => v.GroupBy(st => st.Data.Route)
                    .SelectMany(gr => gr.GroupBy(st => st.Data.DirectionId)
                        .Select(g => g.OrderBy(st => st.Data.DepartureTime)
                            .First().Id))
                    .ToList());

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
                    .SelectMany(gr => gr.GroupBy(st => st.Data.DirectionId)
                        .Select(dg => dg.OrderBy(st => st.Data.DepartureTime).First())))
                .ToList();

            return filteredSourceNodes;
        }

        private void MoveAnts()
        {
            foreach (var ant in _ants)
            {
                var currentNodeId = ant.VisitedNodeIds.Last();
                var currentNode = _graph.Nodes[currentNodeId];

                var selectedEdge = SelectNextEdge(currentNode, ant);
                if (selectedEdge != null)
                {
                    ant.VisitNode(selectedEdge.DestinationId, selectedEdge.Cost);
                }
            }
        }

        private GraphEdge<int> SelectNextEdge(GraphNode<int, StopTimeInfo> currentNode, Ant ant)
        {
            if (!currentNode.Neighbors.Any())
            {
                return null;
            }

            var edgeWithCoefficients = currentNode.Neighbors
                .Where(n => !ant.VisitedNodeIds.Contains(n.DestinationId))
                .ToDictionary(n => n, n => GetEdgeCoefficient(currentNode.Id, n));

            var bestEdge = edgeWithCoefficients.OrderBy(n => n.Value).First().Key;

            return bestEdge;
        }

        private void UpdatePheromoneTrails()
        {
            EvaporatePheromone();
            AddAntPheromones();
        }
    }
}