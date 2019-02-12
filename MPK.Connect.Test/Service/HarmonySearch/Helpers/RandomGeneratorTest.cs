using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;

namespace MPK.Connect.Test.Service.HarmonySearch.Helpers
{
    [TestClass]
    public class RandomGeneratorTest : IDisposable
    {
        private BoundedRandom _boundedRandom;

        public RandomGeneratorTest()
        {
            _boundedRandom = new BoundedRandom();
        }

        public void Dispose()
        {
            _boundedRandom = null;
        }

        [TestMethod]
        public void TestNextDoubleValue()
        {
            //Arrange
            const double origin = -10;
            const double bound = -9;

            //Act
            var result = _boundedRandom.NextValue(origin, bound);

            //Assert
            Assert.IsTrue(result >= origin);
            Assert.IsTrue(result <= bound);
        }
    }
}