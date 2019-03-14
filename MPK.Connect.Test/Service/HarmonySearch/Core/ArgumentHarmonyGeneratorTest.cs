using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Generator;
using static MPK.Connect.Service.Business.HarmonySearch.Constants.HarmonySearchConstants;

namespace MPK.Connect.Test.Service.HarmonySearch.Core
{
    [TestClass]
    public class ArgumentHarmonyGeneratorTest : IDisposable
    {
        private readonly DoubleArgumentObjectiveFunction _function;
        private ContinuousArgumentHarmonyGenerator<double> _argumentHarmonyGenerator;

        public ArgumentHarmonyGeneratorTest()
        {
            var harmonyMemory = new HarmonyMemory<double>(DefaultHarmonyMemorySize);

            _function = new DoubleArgumentObjectiveFunction();
            _argumentHarmonyGenerator = new ContinuousArgumentHarmonyGenerator<double>(_function, harmonyMemory, DefaultHarmonyMemoryConsiderationRatio, DefaultPitchAdjustmentRatio);
        }

        [TestCleanup]
        public void Dispose()
        {
            _argumentHarmonyGenerator = null;
        }

        [TestMethod]
        public void TestCalculateSolution()
        {
            //Arrange
            var x1 = 2;
            var x2 = 0.7;
            var expectedValue = _function.CalculateObjectiveValue(x1, x2);

            //Act
            var result = _argumentHarmonyGenerator.GetHarmony(x1, x2);

            //Assert
            Assert.AreEqual(expectedValue, result.ObjectiveValue);
        }

        [TestMethod]
        public void TestEstablishArgumentGenerationRuleForMemoryConsideration()
        {
            //Arrange
            var value = DefaultHarmonyMemoryConsiderationRatio * (1 - DefaultPitchAdjustmentRatio);

            //Act
            var result = _argumentHarmonyGenerator.EstablishHarmonyGenerationRule(value);

            //Assert
            Assert.AreEqual(HarmonyGenerationRules.MemoryConsideration, result);
        }

        [TestMethod]
        public void TestEstablishArgumentGenerationRuleForPitchAdjusting()
        {
            //Arrange
            const double value = DefaultHarmonyMemoryConsiderationRatio * DefaultPitchAdjustmentRatio - 0.01;

            //Act
            var result = _argumentHarmonyGenerator.EstablishHarmonyGenerationRule(value);

            //Assert
            Assert.AreEqual(HarmonyGenerationRules.PitchAdjustment, result);
        }

        [TestMethod]
        public void TestEstablishArgumentGenerationRuleForRandomChoosing()
        {
            //Arrange
            const double value = 0.96;

            //Act
            var result = _argumentHarmonyGenerator.EstablishHarmonyGenerationRule(value);

            //Assert
            Assert.AreEqual(HarmonyGenerationRules.RandomChoosing, result);
        }

        [TestMethod]
        public void TestGenerateRandomArguments()
        {
            //Arrange

            //Act
            var result = _argumentHarmonyGenerator.GenerateRandomArguments();

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
            var result = _argumentHarmonyGenerator.GenerateRandomHarmony();

            //Assert
            Assert.AreEqual(_function.GetArgumentsCount(), result.Arguments.Length);
            Assert.AreEqual(_argumentHarmonyGenerator.GetHarmony(result.Arguments).ObjectiveValue, result.ObjectiveValue);
            Assert.IsTrue(_function.IsWithinBounds(result.Arguments[0], 0));
            Assert.IsTrue(_function.IsWithinBounds(result.Arguments[1], 1));
        }

        [TestMethod]
        public void TestImproviseArguments()
        {
            //Arrange
            var harmonyMemory = new HarmonyMemory<double>(3);
            _argumentHarmonyGenerator.HarmonyMemory = harmonyMemory;
            for (var i = 0; i < harmonyMemory.MaxCapacity; i++)
            {
                var randomSolution = _argumentHarmonyGenerator.GenerateRandomHarmony();
                harmonyMemory.Add(randomSolution);
            }

            //Act
            var result = _argumentHarmonyGenerator.ImproviseArguments();

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
            _argumentHarmonyGenerator.HarmonyMemory = harmonyMemory;
            for (var i = 0; i < harmonyMemory.MaxCapacity; i++)
            {
                var randomSolution = _argumentHarmonyGenerator.GenerateRandomHarmony();
                harmonyMemory.Add(randomSolution);
            }

            //Act
            var result = _argumentHarmonyGenerator.ImproviseHarmony();

            //Assert
            Assert.AreEqual(_argumentHarmonyGenerator.GetHarmony(result.Arguments).ObjectiveValue, result.ObjectiveValue);
            Assert.AreEqual(_function.GetArgumentsCount(), result.Arguments.Length);
        }

        [TestMethod]
        public void TestUseMemoryConsideration()
        {
            //Arrange
            var harmonyMemory = new HarmonyMemory<double>(3);
            for (var i = 0; i < harmonyMemory.MaxCapacity; i++)
            {
                var randomSolution = _argumentHarmonyGenerator.GenerateRandomHarmony();
                harmonyMemory.Add(randomSolution);
            }

            _argumentHarmonyGenerator.HarmonyMemory = harmonyMemory;
            const int argumentIndex = 0;

            //Act
            var result = _argumentHarmonyGenerator.UseMemoryConsideration(argumentIndex);

            //Assert
            Assert.IsTrue(harmonyMemory.GetArguments(argumentIndex).Contains(result));
        }

        [TestMethod]
        public void TestUsePitchAdjusting()
        {
            //Arrange
            var harmonyMemory = new HarmonyMemory<double>(3);
            _argumentHarmonyGenerator.HarmonyMemory = harmonyMemory;
            for (var i = 0; i < harmonyMemory.MaxCapacity; i++)
            {
                var randomSolution = _argumentHarmonyGenerator.GenerateRandomHarmony();
                harmonyMemory.Add(randomSolution);
            }
            const int argumentIndex = 0;

            //Act
            var result = _argumentHarmonyGenerator.UsePitchAdjustment(argumentIndex);

            //Assert
            Assert.IsFalse(harmonyMemory.GetArguments(argumentIndex).Contains(result));
        }
    }
}