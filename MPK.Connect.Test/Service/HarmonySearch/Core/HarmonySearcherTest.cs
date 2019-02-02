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
        private HarmonySearcher<double> _harmonySearcher;

        public HarmonySearcherTest()
        {
            _harmonySearcher = new HarmonySearcher<double>(new DoubleObjectiveFunction(), new RandomGenerator());
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
        public void TestSearchForHarmony()
        {
            //Arrange

            //Act
            var solution = _harmonySearcher.SearchForHarmony();

            //Assert
            Assert.AreEqual(_harmonySearcher.MaxImprovisationCount, _harmonySearcher.ImprovisationCount);
            Assert.AreEqual(0, solution.ObjectiveValue);
        }
    }
}