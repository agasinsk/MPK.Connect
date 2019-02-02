using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;

namespace MPK.Connect.Test.Service.HarmonySearch.Helpers
{
    [TestClass]
    public class RandomGeneratorTest : IDisposable
    {
        private RandomGenerator _randomGenerator;

        public RandomGeneratorTest()
        {
            _randomGenerator = new RandomGenerator();
        }

        public void Dispose()
        {
            _randomGenerator = null;
        }

        [TestMethod]
        public void TestNextDoubleValue()
        {
            //Arrange
            const double origin = -10;
            const double bound = -9;

            //Act
            var result = _randomGenerator.NextValue(origin, bound);

            //Assert
            Assert.IsTrue(result >= origin);
            Assert.IsTrue(result <= bound);
        }
    }
}