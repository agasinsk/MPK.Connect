using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Business.HarmonySearch;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using TspLibNet.Graph.Nodes;

namespace MPK.Connect.Test.Service.HarmonySearch.Core
{
    [TestClass]
    public class DiscreteHarmonySearcherTest : IDisposable
    {
        private const int HarmonyMemorySize = 10;
        private readonly TravelingSalesmanObjectiveFunction _objectiveFunction;
        private HarmonySearcher<INode> _harmonySearcher;

        public DiscreteHarmonySearcherTest()
        {
            _objectiveFunction = new TravelingSalesmanObjectiveFunction();
            _harmonySearcher = new HarmonySearcher<INode>(_objectiveFunction, HarmonyMemorySize, 10000, 0.92, 0.35);
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
            Assert.AreEqual(HarmonyMemorySize, harmonyMemory.Count);
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
            Assert.AreEqual(HarmonyMemorySize, _harmonySearcher.HarmonyMemory.Count);
            Assert.AreEqual(_harmonySearcher.MaxImprovisationCount, _harmonySearcher.ImprovisationCount);

            Assert.AreEqual(expectedObjectiveValue, optimalSolution.ObjectiveValue);

            CollectionAssert.AreEqual(expectedArguments, optimalSolution.Arguments);
        }
    }
}