using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Business.HarmonySearch;
using TspLibNet.Graph.Nodes;
using static MPK.Connect.Service.Business.HarmonySearch.Constants.HarmonySearchConstants;

namespace MPK.Connect.Test.Service.HarmonySearch.Core
{
    [TestClass]
    public class DiscreteHarmonyGeneratorTest : IDisposable
    {
        private readonly TravelingSalesmanObjectiveFunction _function;
        private DiscreteHarmonyGenerator<INode> _discreteHarmonyGenerator;

        public DiscreteHarmonyGeneratorTest()
        {
            var harmonyMemory = new HarmonyMemory<INode>(DefaultHarmonyMemorySize);

            _function = new TravelingSalesmanObjectiveFunction();
            _discreteHarmonyGenerator = new DiscreteHarmonyGenerator<INode>(_function, harmonyMemory,
                DefaultHarmonyMemoryConsiderationRatio, DefaultPitchAdjustmentRatio);
        }

        [TestCleanup]
        public void Dispose()
        {
            _discreteHarmonyGenerator = null;
        }

        [TestMethod]
        public void TestCalculateDiscreteSolution()
        {
            //Arrange
            var nodes = new List<INode>();
            var expectedValue = _function.CalculateObjectiveValue(nodes.ToArray());

            //Act
            var result = _discreteHarmonyGenerator.GetHarmony(nodes.ToArray());

            //Assert
            Assert.AreEqual(expectedValue, result.ObjectiveValue);
        }

        [TestMethod]
        public void TestGenerateRandomDiscreteArguments()
        {
            //Arrange

            //Act
            var result = _discreteHarmonyGenerator.GenerateRandomArguments();

            //Assert
            Assert.AreEqual(_function.GetArgumentsCount(), result.Length);
            Assert.AreEqual(result.Distinct().Count(), result.Count());

            Assert.IsTrue(result.All(n => _function.ProblemItem.Problem.NodeProvider.GetNodes().Any(g => g.Id == n.Id)));
            Assert.IsTrue(result.All(n => _function.ProblemItem.Problem.NodeProvider.GetNodes().Count(g => g.Id == n.Id) == 1));
        }

        [TestMethod]
        public void TestGenerateRandomDiscreteSolution()
        {
            //Arrange

            //Act
            var result = _discreteHarmonyGenerator.GenerateRandomHarmony();

            //Assert
            Assert.AreEqual(_function.GetArgumentsCount(), result.Arguments.Length);
            Assert.AreEqual(_discreteHarmonyGenerator.GetHarmony(result.Arguments).ObjectiveValue, result.ObjectiveValue);
            Assert.IsTrue(result.Arguments.All(n => _function.ProblemItem.Problem.NodeProvider.GetNodes().Any(g => g.Id == n.Id)));
        }

        [TestMethod]
        public void TestImproviseArguments()
        {
            //Arrange
            for (var i = 0; i < _discreteHarmonyGenerator.HarmonyMemory.MaxCapacity; i++)
            {
                var randomSolution = _discreteHarmonyGenerator.GenerateRandomHarmony();
                _discreteHarmonyGenerator.HarmonyMemory.Add(randomSolution);
            }

            //Act
            var result = _discreteHarmonyGenerator.ImproviseArguments();

            //Assert
            Assert.AreEqual(_function.GetArgumentsCount(), result.Length);
            Assert.AreEqual(result.Select(s => s.Id).Distinct().Count(), result.Length);

            Assert.IsTrue(result.All(n => _function.ProblemItem.Problem.NodeProvider.GetNodes().Any(g => g.Id == n.Id)));
            Assert.IsTrue(result.All(n => _function.ProblemItem.Problem.NodeProvider.GetNodes().Count(g => g.Id == n.Id) == 1));
        }

        [TestMethod]
        public void TestImproviseSolution()
        {
            //Arrange
            for (var i = 0; i < _discreteHarmonyGenerator.HarmonyMemory.MaxCapacity; i++)
            {
                var randomSolution = _discreteHarmonyGenerator.GenerateRandomHarmony();
                _discreteHarmonyGenerator.HarmonyMemory.Add(randomSolution);
            }
            //Act
            var result = _discreteHarmonyGenerator.ImproviseHarmony();

            //Assert
            Assert.AreEqual(_discreteHarmonyGenerator.GetHarmony(result.Arguments).ObjectiveValue, result.ObjectiveValue);
            Assert.AreEqual(_function.GetArgumentsCount(), result.Arguments.Length);

            Assert.AreEqual(result.Arguments.Select(s => s.Id).Distinct().Count(), result.Arguments.Length);
            Assert.IsTrue(result.Arguments.All(n => _function.ProblemItem.Problem.NodeProvider.GetNodes().Any(g => g.Id == n.Id)));
            Assert.IsTrue(result.Arguments.All(n => _function.ProblemItem.Problem.NodeProvider.GetNodes().Count(g => g.Id == n.Id) == 1));
        }

        [TestMethod]
        public void TestUseDiscreteMemoryConsideration()
        {
            //Arrange
            var harmonyMemory = new HarmonyMemory<INode>(3);
            for (var i = 0; i < harmonyMemory.MaxCapacity; i++)
            {
                var randomSolution = _discreteHarmonyGenerator.GenerateRandomHarmony();
                harmonyMemory.Add(randomSolution);
            }

            _discreteHarmonyGenerator.HarmonyMemory = harmonyMemory;
            const int argumentIndex = 0;

            //Act
            var result = _discreteHarmonyGenerator.UseMemoryConsideration(argumentIndex);

            //Assert
            Assert.IsTrue(harmonyMemory.GetArguments(argumentIndex).Contains(result));
        }
    }
}