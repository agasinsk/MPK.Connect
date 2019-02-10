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
    public class TravelingSalesmanObjectiveFunction : NodeCalculator, IObjectiveFunction<INode>
    {
        public static string DefaultProblemName = "berlin52";
        private const double MaxPitchAdjustingIndex = 0.2;
        private const string RootDir = @"..\..\..\TSPLIB95";

        private readonly List<INode> _nodes;
        private readonly IRandomGenerator<double> _random;
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
            _random = new RandomGenerator();
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

        public INode GetArgumentValue(int argumentIndex, int? discreteValueIndex = null)
        {
            if (discreteValueIndex.HasValue)
            {
                var node = _nodes[discreteValueIndex.Value];
                return _unusedNodes.Contains(node) ? node : null;
            }

            var randomIndex = _random.Next(0, _unusedNodes.Count);
            var randomNode = _unusedNodes.ElementAt(randomIndex);

            return randomNode;
        }

        public int GetIndexOfDiscreteValue(int argumentIndex, INode argumentValue)
        {
            return _nodes.IndexOf(argumentValue);
        }

        public INode GetLowerBound(int argumentIndex)
        {
            throw new NotImplementedException();
        }

        public INode GetMaximumContinuousPitchAdjustmentProportion()
        {
            throw new NotImplementedException();
        }

        public int GetMaximumDiscretePitchAdjustmentIndex()
        {
            return (int)Math.Ceiling(MaxPitchAdjustingIndex * _unusedNodes.Count);
        }

        public int GetPossibleDiscreteValuesCount(int argumentIndex)
        {
            return _nodes.Count;
        }

        public INode GetUpperBound(int argumentIndex)
        {
            throw new NotImplementedException();
        }

        public bool IsArgumentDiscrete(int argumentIndex)
        {
            return true;
        }

        public bool IsArgumentValuePossible(INode argumentValue)
        {
            return _unusedNodes.Contains(argumentValue);
        }

        public bool IsArgumentVariable(int argumentIndex)
        {
            return true;
        }

        public void SaveArgumentValue(int argumentIndex, INode argumentValue)
        {
            _unusedNodes.Remove(argumentValue);
            if (argumentIndex == _nodes.Count - 1)
            {
                _unusedNodes = new List<INode>(_nodes);
            }
        }
    }
}