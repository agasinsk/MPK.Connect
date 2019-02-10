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
        private HarmonyGenerator<INode> _harmonyGenerator;

        public DiscreteHarmonyGeneratorTest()
        {
            var harmonyMemory = new HarmonyMemory<INode>(DefaultHarmonyMemorySize);

            _function = new TravelingSalesmanObjectiveFunction();
            _harmonyGenerator = new HarmonyGenerator<INode>(_function, new NodeRandomGenerator(), harmonyMemory,
                DefaultHarmonyMemoryConsiderationRatio, DefaultPitchAdjustmentRatio);
        }

        [TestCleanup]
        public void Dispose()
        {
            _harmonyGenerator = null;
        }

        [TestMethod]
        public void TestCalculateDiscreteSolution()
        {
            //Arrange
            var nodes = new List<INode>();
            var expectedValue = _function.CalculateObjectiveValue(nodes.ToArray());

            //Act
            var result = _harmonyGenerator.CalculateSolution(nodes.ToArray());

            //Assert
            Assert.AreEqual(expectedValue, result.ObjectiveValue);
        }

        [TestMethod]
        public void TestGenerateRandomDiscreteArguments()
        {
            //Arrange

            //Act
            var result = _harmonyGenerator.GenerateRandomArguments();

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
            var result = _harmonyGenerator.GenerateRandomSolution();

            //Assert
            Assert.AreEqual(_function.GetArgumentsCount(), result.Arguments.Length);
            Assert.AreEqual(_harmonyGenerator.CalculateSolution(result.Arguments).ObjectiveValue, result.ObjectiveValue);
            Assert.IsTrue(result.Arguments.All(n => _function.ProblemItem.Problem.NodeProvider.GetNodes().Any(g => g.Id == n.Id)));
        }

        [TestMethod]
        public void TestImproviseArguments()
        {
            //Arrange
            var harmonyMemory = new HarmonyMemory<INode>(3);
            _harmonyGenerator.HarmonyMemory = harmonyMemory;
            for (var i = 0; i < harmonyMemory.MaxCapacity; i++)
            {
                var randomSolution = _harmonyGenerator.GenerateRandomSolution();
                harmonyMemory.Add(randomSolution);
            }

            //Act
            var result = _harmonyGenerator.ImproviseArguments();

            //Assert
            Assert.AreEqual(_function.GetArgumentsCount(), result.Length);
            Assert.AreEqual(_harmonyGenerator.ArgumentsCount, result.Length);
            Assert.AreEqual(result.Select(s => s.Id).Distinct().Count(), result.Length);

            Assert.IsTrue(result.All(n => _function.ProblemItem.Problem.NodeProvider.GetNodes().Any(g => g.Id == n.Id)));
            Assert.IsTrue(result.All(n => _function.ProblemItem.Problem.NodeProvider.GetNodes().Count(g => g.Id == n.Id) == 1));
        }

        [TestMethod]
        public void TestImproviseSolution()
        {
            //Arrange
            var harmonyMemory = new HarmonyMemory<INode>(3);
            _harmonyGenerator.HarmonyMemory = harmonyMemory;
            for (var i = 0; i < harmonyMemory.MaxCapacity; i++)
            {
                var randomSolution = _harmonyGenerator.GenerateRandomSolution();
                harmonyMemory.Add(randomSolution);
            }

            //Act
            var result = _harmonyGenerator.ImproviseHarmony();

            //Assert
            Assert.AreEqual(_harmonyGenerator.CalculateSolution(result.Arguments).ObjectiveValue, result.ObjectiveValue);
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
                var randomSolution = _harmonyGenerator.GenerateRandomSolution();
                harmonyMemory.Add(randomSolution);
            }

            _harmonyGenerator.HarmonyMemory = harmonyMemory;
            const int argumentIndex = 0;

            //Act
            var result = _harmonyGenerator.UseMemoryConsideration(argumentIndex);

            //Assert
            Assert.IsTrue(harmonyMemory.GetArguments(argumentIndex).Contains(result));
        }

        [TestMethod]
        public void TestUseDiscretePitchAdjusting()
        {
            //Arrange
            var harmonyMemory = new HarmonyMemory<INode>(3);
            _harmonyGenerator.HarmonyMemory = harmonyMemory;
            for (var i = 0; i < harmonyMemory.MaxCapacity; i++)
            {
                var randomSolution = _harmonyGenerator.GenerateRandomSolution();
                harmonyMemory.Add(randomSolution);
            }
            const int argumentIndex = 0;

            //Act
            var result = _harmonyGenerator.UsePitchAdjustment(argumentIndex);

            //Assert
            Assert.IsFalse(harmonyMemory.GetArguments(argumentIndex).Contains(result));
        }
    }
}