using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Business.HarmonySearch;

namespace MPK.Connect.Test.Service.HarmonySearch.Core
{
    [TestClass]
    public class HarmonyMemoryTest : IDisposable
    {
        private readonly int _harmonyMemoryTestSize;
        private HarmonyMemory<double> _harmonyMemory;

        public HarmonyMemoryTest()
        {
            _harmonyMemoryTestSize = 3;
            _harmonyMemory = new HarmonyMemory<double>(_harmonyMemoryTestSize);
        }

        [TestCleanup]
        public void Dispose()
        {
            _harmonyMemory = null;
        }

        [TestMethod]
        public void TestAddRangeIfMemoryIsFull()
        {
            //Arrange
            var harmonies = new List<Harmony<double>>
            {
                new Harmony<double>(2, 1, 3),
                new Harmony<double>(4, 3, 3),
                new Harmony<double>(-2, -1, 3),
                new Harmony<double>(-7, 3, 3)
            };

            var testSolution = harmonies.Last();

            //Act
            var result = _harmonyMemory.AddRange(harmonies);

            //Assert
            Assert.IsFalse(result);
            Assert.IsTrue(!_harmonyMemory.Contains(testSolution));
        }

        [TestMethod]
        public void TestAddSolutionIfMemoryIsFull()
        {
            //Arrange
            _harmonyMemory.Add(new Harmony<double>(2, 1, 3));
            _harmonyMemory.Add(new Harmony<double>(-2, -1, 3));
            _harmonyMemory.Add(new Harmony<double>(-4, -1, 3));

            var testSolution = new Harmony<double>(-7, 3, 3);

            //Act
            var result = _harmonyMemory.Add(testSolution);

            //Assert
            Assert.IsFalse(result);
            Assert.IsTrue(!_harmonyMemory.Contains(testSolution));
        }

        [TestMethod]
        public void TestGetArgumentFromRandomHarmony()
        {
            // Arrange
            var harmonies = new List<Harmony<double>>
            {
                new Harmony<double>(2, 1, 3),
                new Harmony<double>(4, 3, 3),
                new Harmony<double>(-2, -1, 3)
            };

            _harmonyMemory.AddRange(harmonies);

            var argumentIndex = 1;

            // Act
            var argument = _harmonyMemory.GetArgumentFromRandomHarmony(argumentIndex);

            // Assert
            Assert.IsTrue(harmonies.Select(h => h.Arguments[1]).Contains(argument));
        }

        [TestMethod]
        public void TestGetBestSolution()
        {
            //Arrange
            var newBest = new Harmony<double>(-7, 3, 3);
            _harmonyMemory.Add(new Harmony<double>(2, 1, 3));
            _harmonyMemory.Add(new Harmony<double>(-2, -1, 3));
            _harmonyMemory.Add(newBest);

            //Act
            var best = _harmonyMemory.BestHarmony;

            //Assert
            Assert.AreEqual(newBest, best);
        }

        [TestMethod]
        public void TestGetMaxCapacity()
        {
            //Arrange

            //Act
            var capacity = _harmonyMemory.MaxCapacity;

            //Assert
            Assert.AreEqual(_harmonyMemoryTestSize, capacity);
        }

        [TestMethod]
        public void TestGetSizeIfEmpty()
        {
            //Arrange

            //Act
            var size = _harmonyMemory.Count;

            //Assert
            Assert.AreEqual(0, size);
        }

        [TestMethod]
        public void TestGetSizeIfNotEmpty()
        {
            //Arrange
            _harmonyMemory.Add(new Harmony<double>(22, 0, 11));

            //Act
            var size = _harmonyMemory.Count;

            //Assert
            Assert.AreEqual(1, size);
        }

        [TestMethod]
        public void TestGetWorstSolution()
        {
            //Arrange
            var newWorst = new Harmony<double>(4, 3, 3);
            _harmonyMemory.Add(new Harmony<double>(2, 1, 3));
            _harmonyMemory.Add(new Harmony<double>(-2, -1, 3));
            _harmonyMemory.Add(newWorst);

            //Act
            var worst = _harmonyMemory.WorstHarmony;

            //Assert
            Assert.AreEqual(newWorst, worst);
        }

        [TestMethod]
        public void TestSwapWithWorstSolutionForNewBestSolution()
        {
            //Arrange
            var oldWorstSolution = new Harmony<double>(4, 3, 3);
            _harmonyMemory.Add(new Harmony<double>(2, 1, 3));
            _harmonyMemory.Add(new Harmony<double>(-2, -1, 3));
            _harmonyMemory.Add(oldWorstSolution);

            var newSolution = new Harmony<double>(-10, 2, 2.5);

            //Act
            _harmonyMemory.SwapWithWorstHarmony(newSolution);

            //Assert
            Assert.AreEqual(newSolution, _harmonyMemory.BestHarmony);
            Assert.IsFalse(_harmonyMemory.Contains(oldWorstSolution));
        }

        [TestMethod]
        public void TestSwapWithWorstSolutionForNewMiddleSolution()
        {
            //Arrange
            var oldWorstSolution = new Harmony<double>(4, 3, 3);
            _harmonyMemory.Add(new Harmony<double>(2, 1, 3));
            _harmonyMemory.Add(new Harmony<double>(-2, -1, 3));
            _harmonyMemory.Add(oldWorstSolution);

            var newSolution = new Harmony<double>(0, 2, 2.5);

            //Act
            _harmonyMemory.SwapWithWorstHarmony(newSolution);

            //Assert
            Assert.IsTrue(_harmonyMemory.Contains(newSolution));
            Assert.IsTrue(!_harmonyMemory.Contains(oldWorstSolution));
        }

        [TestMethod]
        public void TestSwapWithWorstSolutionForNewWorstSolution()
        {
            //Arrange
            var oldWorstSolution = new Harmony<double>(4, 3, 3);
            _harmonyMemory.Add(new Harmony<double>(2, 1, 3));
            _harmonyMemory.Add(new Harmony<double>(-2, -1, 3));
            _harmonyMemory.Add(oldWorstSolution);

            var newSolution = new Harmony<double>(3, 2, 2.5);

            //Act
            _harmonyMemory.SwapWithWorstHarmony(newSolution);

            //Assert
            Assert.AreEqual(newSolution, _harmonyMemory.WorstHarmony);
            Assert.IsTrue(!_harmonyMemory.Contains(oldWorstSolution));
        }
    }
}