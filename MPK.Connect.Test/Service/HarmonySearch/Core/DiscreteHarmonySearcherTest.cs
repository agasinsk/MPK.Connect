using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Business.HarmonySearch;
using TspLibNet.Graph.Nodes;

namespace MPK.Connect.Test.Service.HarmonySearch.Core
{
    [TestClass]
    public class DiscreteHarmonySearcherTest : IDisposable
    {
        private readonly TravelingSalesmanObjectiveFunction _objectiveFunction;
        private HarmonySearcher<INode> _harmonySearcher;

        public DiscreteHarmonySearcherTest()
        {
            _objectiveFunction = new TravelingSalesmanObjectiveFunction();
            _harmonySearcher = new HarmonySearcher<INode>(_objectiveFunction, new NodeRandomGenerator(), 10, 10000, 0.92, 0.35, 0.99);
        }

        public void Dispose()
        {
            _harmonySearcher = null;
        }

        [TestMethod]
        public void TestInitializeHarmonyMemory()
        {
            //Arrange

            //Act
            _harmonySearcher.InitializeHarmonyMemory();
            var harmonyMemory = _harmonySearcher.HarmonyMemory;

            //Assert
            Assert.AreEqual(10, harmonyMemory.Count);
            Assert.IsTrue(harmonyMemory.BestHarmony.ObjectiveValue <= harmonyMemory.WorstHarmony.ObjectiveValue);
        }

        [TestMethod]
        public void TestSearchForHarmony()
        {
            // Arrange
            var expectedObjectiveValue = _objectiveFunction.ProblemItem.OptimalTourDistance;
            var expectedArguments = _objectiveFunction.ProblemItem.OptimalTour.Nodes;

            // Act
            var optimalSolution = _harmonySearcher.SearchForHarmony();

            // Assert
            Assert.AreEqual(10, _harmonySearcher.HarmonyMemory.Count);
            Assert.AreEqual(_harmonySearcher.MaxImprovisationCount, _harmonySearcher.ImprovisationCount);

            Assert.AreEqual(expectedObjectiveValue, optimalSolution.ObjectiveValue);

            CollectionAssert.AreEqual(expectedArguments, optimalSolution.Arguments);
        }
    }
}