using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using static MPK.Connect.Service.Business.HarmonySearch.Constants.HarmonySearchConstants;

namespace MPK.Connect.Test.Service.HarmonySearch.Core
{
    [TestClass]
    public class HarmonySearcherTest : IDisposable
    {
        private readonly DoubleArgumentObjectiveFunction _objectiveFunction;
        private HarmonySearcher<double> _harmonySearcher;

        public HarmonySearcherTest()
        {
            _objectiveFunction = new DoubleArgumentObjectiveFunction();
            _harmonySearcher = new HarmonySearcher<double>(_objectiveFunction);
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
            Assert.AreEqual(DefaultHarmonyMemorySize, harmonyMemory.Count);
            Assert.IsTrue(harmonyMemory.BestHarmony.ObjectiveValue <= harmonyMemory.WorstHarmony.ObjectiveValue);
        }

        [TestMethod]
        public void TestSearchForHarmony_FirstQuarter()
        {
            //Arrange
            var expectedArguments = new[] { 0.55672, 0.55672 };
            _objectiveFunction.SetLowerBound(0, 0);
            _objectiveFunction.SetUpperBound(0, 1);
            _objectiveFunction.SetLowerBound(1, 0);
            _objectiveFunction.SetUpperBound(1, 1);

            //Act
            var optimalSolution = _harmonySearcher.SearchForHarmony();

            //Assert
            Assert.AreEqual(DefaultHarmonyMemorySize, _harmonySearcher.HarmonyMemory.Count);
            Assert.AreEqual(_harmonySearcher.MaxImprovisationCount, _harmonySearcher.ImprovisationCount);

            Assert.AreEqual(-0.19219, optimalSolution.ObjectiveValue, 4);

            Assert.AreEqual(expectedArguments[0], optimalSolution.Arguments[0], 4);
            Assert.AreEqual(expectedArguments[1], optimalSolution.Arguments[1], 4);
        }

        [TestMethod]
        public void TestSearchForHarmony_FourthQuarter()
        {
            //Arrange
            var expectedArguments = new[] { 0.55672, -0.55672 };
            _objectiveFunction.SetLowerBound(0, 0);
            _objectiveFunction.SetUpperBound(0, 1);
            _objectiveFunction.SetLowerBound(1, -1);
            _objectiveFunction.SetUpperBound(1, 0);

            //Act
            var optimalSolution = _harmonySearcher.SearchForHarmony();

            //Assert
            Assert.AreEqual(DefaultHarmonyMemorySize, _harmonySearcher.HarmonyMemory.Count);
            Assert.AreEqual(_harmonySearcher.MaxImprovisationCount, _harmonySearcher.ImprovisationCount);

            Assert.AreEqual(-0.19219, optimalSolution.ObjectiveValue, 4);

            Assert.AreEqual(expectedArguments[0], optimalSolution.Arguments[0], 4);
            Assert.AreEqual(expectedArguments[1], optimalSolution.Arguments[1], 4);
        }

        [TestMethod]
        public void TestSearchForHarmony_SecondQuarter()
        {
            //Arrange
            var expectedArguments = new[] { -0.55672, 0.55672 };
            _objectiveFunction.SetLowerBound(0, -1);
            _objectiveFunction.SetUpperBound(0, 0);
            _objectiveFunction.SetLowerBound(1, 0);
            _objectiveFunction.SetUpperBound(1, 1);

            //Act
            var optimalSolution = _harmonySearcher.SearchForHarmony();

            //Assert
            Assert.AreEqual(DefaultHarmonyMemorySize, _harmonySearcher.HarmonyMemory.Count);
            Assert.AreEqual(_harmonySearcher.MaxImprovisationCount, _harmonySearcher.ImprovisationCount);

            Assert.AreEqual(-0.19219, optimalSolution.ObjectiveValue, 4);

            Assert.AreEqual(expectedArguments[0], optimalSolution.Arguments[0], 4);
            Assert.AreEqual(expectedArguments[1], optimalSolution.Arguments[1], 4);
        }

        [TestMethod]
        public void TestSearchForHarmony_ThirdQuarter()
        {
            //Arrange
            var expectedArguments = new[] { -0.55672, -0.55672 };
            _objectiveFunction.SetLowerBound(0, -1);
            _objectiveFunction.SetUpperBound(0, 0);
            _objectiveFunction.SetLowerBound(1, -1);
            _objectiveFunction.SetUpperBound(1, 0);

            //Act
            var optimalSolution = _harmonySearcher.SearchForHarmony();

            //Assert
            Assert.AreEqual(DefaultHarmonyMemorySize, _harmonySearcher.HarmonyMemory.Count);
            Assert.AreEqual(_harmonySearcher.MaxImprovisationCount, _harmonySearcher.ImprovisationCount);

            Assert.AreEqual(-0.19219, optimalSolution.ObjectiveValue, 4);

            Assert.AreEqual(expectedArguments[0], optimalSolution.Arguments[0], 4);
            Assert.AreEqual(expectedArguments[1], optimalSolution.Arguments[1], 4);
        }
    }
}