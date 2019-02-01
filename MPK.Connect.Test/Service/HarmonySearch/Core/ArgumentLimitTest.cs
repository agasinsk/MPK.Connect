using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Business.HarmonySearch;

namespace MPK.Connect.Test.Service.HarmonySearch.Core
{
    public class ArgumentLimitTest
    {
        [TestMethod]
        public void TestIsWithinLimits()
        {
            //Arrange
            const double number = 11.22;
            var limit = new ArgumentLimit(-10, 15);

            //Act
            var result = limit.IsWithinLimits(number);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        internal void TestIsOutOfLimits()
        {
            //Arrange
            const double number = 11.22;
            var limit = new ArgumentLimit(14, 15);

            //Act
            var result = limit.IsWithinLimits(number);

            //Assert
            Assert.IsFalse(result);
        }
    }
}