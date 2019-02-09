using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Business.HarmonySearch;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;
using static MPK.Connect.Service.Business.HarmonySearch.Constants.HarmonySearchConstants;

namespace MPK.Connect.Test.Service.HarmonySearch.Core
{
    [TestClass]
    public class HarmonySearcherTest : IDisposable
    {
        private readonly DoubleObjectiveFunction _objectiveFunction;
        private HarmonySearcher<double> _harmonySearcher;

        public HarmonySearcherTest()
        {
            _objectiveFunction = new DoubleObjectiveFunction();
            _harmonySearcher = new HarmonySearcher<double>(_objectiveFunction, new RandomGenerator());
        }

        public void Dispose()
        {
            _harmonySearcher = null;
        }

        [TestMethod]
        public void TestGetCurrentPitchAdjustingRatioWhenImprovedScenarioIsOff()
        {
            // Arrange
            var iterationIndex = 100;

            // Act
            var result = _harmonySearcher.GetCurrentPitchAdjustingRatio(iterationIndex);

            // Assert
            Assert.AreEqual(_harmonySearcher.PitchAdjustmentRatio, result);
            Assert.AreEqual(DefaultPitchAdjustmentRatio, result);
        }

        [TestMethod]
        public void TestGetCurrentPitchAdjustingRatioWhenImprovedScenarioIsOn()
        {
            // Arrange
            var iterationIndex = 100;
            var improvedHarmonySearcher = new HarmonySearcher<double>(_objectiveFunction, new RandomGenerator(), true);
            var expectedResult = DefaultMaxPitchAdjustmentRatio -
                                 (DefaultMaxPitchAdjustmentRatio - DefaultMinPitchAdjustmentRatio) * iterationIndex /
                                 DefaultMaxImprovisationCount;

            // Act
            var result = improvedHarmonySearcher.GetCurrentPitchAdjustingRatio(iterationIndex);

            // Assert
            Assert.IsNotNull(improvedHarmonySearcher.PitchAdjustmentRatio);
            Assert.IsTrue(improvedHarmonySearcher.ShouldImprovePitchAdjustingScenario);
            Assert.AreEqual(DefaultMinPitchAdjustmentRatio, improvedHarmonySearcher.MinPitchAdjustmentRatio);
            Assert.AreEqual(DefaultMaxPitchAdjustmentRatio, improvedHarmonySearcher.MaxPitchAdjustmentRatio);
            Assert.AreEqual(DefaultMaxPitchAdjustmentRatio, improvedHarmonySearcher.PitchAdjustmentRatio);
            Assert.AreEqual(DefaultHarmonyMemoryConsiderationRatio, improvedHarmonySearcher.HarmonyMemoryConsiderationRatio);

            Assert.AreEqual(expectedResult, result);
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