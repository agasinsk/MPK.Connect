using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Model;
using MPK.Connect.Service.Helpers;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Test
{
    [TestClass]
    public class ObjectExtensionsTests
    {
        [TestMethod]
        public void TestIfGetPropValueReturnsFalseCorrectly()
        {
            // Arrange
            var calendar = new Calendar();

            // Act
            var result = calendar.GetPropValue<bool>("Friday");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestIfGetPropValueReturnsTrueCorrectly()
        {
            // Arrange
            var calendar = new Calendar { Friday = true };

            // Act
            var result = calendar.GetPropValue<bool>("Friday");

            // Assert
            Assert.IsTrue(result);
        }
    }
}