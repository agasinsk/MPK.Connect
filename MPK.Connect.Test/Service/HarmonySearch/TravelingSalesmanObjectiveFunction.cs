using System;
using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Service.Business.HarmonySearch;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;
using TspLibNet;
using TspLibNet.Graph.Nodes;
using TspLibNet.Tours;

namespace MPK.Connect.Test.Service.HarmonySearch
{
    public class TravelingSalesmanObjectiveFunction : IDiscreteObjectiveFunction<INode>
    {
        public static string DefaultProblemName = "berlin52";
        private const string RootDir = @"..\..\..\TSPLIB95";

        private readonly List<INode> _nodes;
        private readonly IRandom _random;
        private List<INode> _unusedNodes;
        public TspLib95Item ProblemItem { get; }

        public TravelingSalesmanObjectiveFunction(string problemName = null)
        {
            var tspLib = new TspLib95(RootDir);
            if (string.IsNullOrEmpty(problemName))
            {
                problemName = DefaultProblemName;
            }

            tspLib.LoadTSP(problemName);
            ProblemItem = tspLib.GetItemByName(problemName, ProblemType.TSP);
            _nodes = ProblemItem.Problem.NodeProvider.GetNodes();
            _unusedNodes = new List<INode>(_nodes);
            _random = new BoundedRandom();
        }

        public double CalculateObjectiveValue(params INode[] arguments)
        {
            var tour = new Tour("HarmonySearchTour", string.Empty, arguments.Length, arguments.Select(n => n.Id));
            return ProblemItem.Problem.TourDistance(tour);
        }

        public int GetArgumentsCount()
        {
            return _nodes.Count;
        }

        public INode GetArgumentValue(int argumentIndex)
        {
            var randomIndex = _random.Next(0, _unusedNodes.Count);
            var randomNode = _unusedNodes.ElementAt(randomIndex);

            return randomNode;
        }

        public int GetIndexOfDiscreteValue(int argumentIndex, INode argumentValue)
        {
            return _nodes.IndexOf(argumentValue);
        }

        public INode GetNeighborValue(int argumentIndex, INode node)
        {
            var distanceProvider = ProblemItem.Problem.EdgeWeightsProvider;

            var neighborNode = _unusedNodes.Aggregate((first, second) =>
                distanceProvider.GetWeight(first, node) < distanceProvider.GetWeight(second, node) ? first : second);
            return neighborNode;
        }

        public bool IsArgumentValuePossible(INode argumentValue)
        {
            return _unusedNodes.Contains(argumentValue);
        }

        public void SaveArgumentValue(int argumentIndex, INode argumentValue)
        {
            if (!_unusedNodes.Remove(argumentValue))
            {
                throw new InvalidOperationException($"Node with id {argumentValue.Id} has been already used!");
            }

            if (argumentIndex == _nodes.Count - 1)
            {
                _unusedNodes = new List<INode>(_nodes);
            }
        }
    }
}