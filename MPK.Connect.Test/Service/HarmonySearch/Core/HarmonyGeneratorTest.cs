using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Business.HarmonySearch;
using static MPK.Connect.Service.Business.HarmonySearch.Constants.HarmonySearchConstants;

namespace MPK.Connect.Test.Service.HarmonySearch.Core
{
    [TestClass]
    public class HarmonyGeneratorTest : IDisposable
    {
        private readonly DoubleObjectiveFunction _function;
        private ContinuousHarmonyGenerator<double> _harmonyGenerator;

        public HarmonyGeneratorTest()
        {
            var harmonyMemory = new HarmonyMemory<double>(DefaultHarmonyMemorySize);

            _function = new DoubleObjectiveFunction();
            _harmonyGenerator = new ContinuousHarmonyGenerator<double>(_function, harmonyMemory, DefaultHarmonyMemoryConsiderationRatio, DefaultPitchAdjustmentRatio);
        }

        [TestCleanup]
        public void Dispose()
        {
            _harmonyGenerator = null;
        }

        [TestMethod]
        public void TestCalculateSolution()
        {
            //Arrange
            var x1 = 2;
            var x2 = 0.7;
            var expectedValue = _function.CalculateObjectiveValue(x1, x2);

            //Act
            var result = _harmonyGenerator.GetHarmony(x1, x2);

            //Assert
            Assert.AreEqual(expectedValue, result.ObjectiveValue);
        }

        [TestMethod]
        public void TestEstablishArgumentGenerationRuleForMemoryConsideration()
        {
            //Arrange
            var value = DefaultHarmonyMemoryConsiderationRatio * (1 - DefaultPitchAdjustmentRatio);

            //Act
            var result = _harmonyGenerator.EstablishArgumentGenerationRule(value);

            //Assert
            Assert.AreEqual(ArgumentGenerationRules.MemoryConsideration, result);
        }

        [TestMethod]
        public void TestEstablishArgumentGenerationRuleForPitchAdjusting()
        {
            //Arrange
            const double value = DefaultHarmonyMemoryConsiderationRatio * DefaultPitchAdjustmentRatio - 0.01;

            //Act
            var result = _harmonyGenerator.EstablishArgumentGenerationRule(value);

            //Assert
            Assert.AreEqual(ArgumentGenerationRules.PitchAdjustment, result);
        }

        [TestMethod]
        public void TestEstablishArgumentGenerationRuleForRandomChoosing()
        {
            //Arrange
            const double value = 0.96;

            //Act
            var result = _harmonyGenerator.EstablishArgumentGenerationRule(value);

            //Assert
            Assert.AreEqual(ArgumentGenerationRules.RandomChoosing, result);
        }

        [TestMethod]
        public void TestGenerateRandomArguments()
        {
            //Arrange

            //Act
            var result = _harmonyGenerator.GenerateRandomArguments();

            //Assert
            Assert.AreEqual(_function.GetArgumentsCount(), result.Length);
            Assert.IsTrue(_function.IsWithinBounds(result[0], 0));
            Assert.IsTrue(_function.IsWithinBounds(result[1], 1));
        }

        [TestMethod]
        public void TestGenerateRandomSolution()
        {
            //Arrange

            //Act
            var result = _harmonyGenerator.GenerateRandomHarmony();

            //Assert
            Assert.AreEqual(_function.GetArgumentsCount(), result.Arguments.Length);
            Assert.AreEqual(_harmonyGenerator.GetHarmony(result.Arguments).ObjectiveValue, result.ObjectiveValue);
            Assert.IsTrue(_function.IsWithinBounds(result.Arguments[0], 0));
            Assert.IsTrue(_function.IsWithinBounds(result.Arguments[1], 1));
        }

        [TestMethod]
        public void TestImproviseArguments()
        {
            //Arrange
            var harmonyMemory = new HarmonyMemory<double>(3);
            _harmonyGenerator.HarmonyMemory = harmonyMemory;
            for (var i = 0; i < harmonyMemory.MaxCapacity; i++)
            {
                var randomSolution = _harmonyGenerator.GenerateRandomHarmony();
                harmonyMemory.Add(randomSolution);
            }

            //Act
            var result = _harmonyGenerator.ImproviseArguments();

            //Assert
            Assert.AreEqual(_function.GetArgumentsCount(), result.Length);
            Assert.IsTrue(_function.IsWithinBounds(result[0], 0));
            Assert.IsTrue(_function.IsWithinBounds(result[1], 1));
        }

        [TestMethod]
        public void TestImproviseSolution()
        {
            //Arrange
            var harmonyMemory = new HarmonyMemory<double>(3);
            _harmonyGenerator.HarmonyMemory = harmonyMemory;
            for (var i = 0; i < harmonyMemory.MaxCapacity; i++)
            {
                var randomSolution = _harmonyGenerator.GenerateRandomHarmony();
                harmonyMemory.Add(randomSolution);
            }

            //Act
            var result = _harmonyGenerator.ImproviseHarmony();

            //Assert
            Assert.AreEqual(_harmonyGenerator.GetHarmony(result.Arguments).ObjectiveValue, result.ObjectiveValue);
            Assert.AreEqual(_function.GetArgumentsCount(), result.Arguments.Length);
        }

        [TestMethod]
        public void TestUseMemoryConsideration()
        {
            //Arrange
            var harmonyMemory = new HarmonyMemory<double>(3);
            for (var i = 0; i < harmonyMemory.MaxCapacity; i++)
            {
                var randomSolution = _harmonyGenerator.GenerateRandomHarmony();
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
        public void TestUsePitchAdjusting()
        {
            //Arrange
            var harmonyMemory = new HarmonyMemory<double>(3);
            _harmonyGenerator.HarmonyMemory = harmonyMemory;
            for (var i = 0; i < harmonyMemory.MaxCapacity; i++)
            {
                var randomSolution = _harmonyGenerator.GenerateRandomHarmony();
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