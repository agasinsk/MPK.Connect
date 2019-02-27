using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Helpers;

namespace MPK.Connect.Test.Service
{
    [TestClass]
    public class DoubleComparerExtensionsTests
    {
        [TestMethod]
        public void LessThan_ShouldReturnFalse_WithBiggerPrecision()
        {
            // Arrange
            var x = 2.192912;
            var y = 2.19281;

            // Act
            var result = x.LessThan(y, .00001);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void LessThan_ShouldReturnFalse_WithDefaultPrecision()
        {
            // Arrange
            var x = 2.192922;
            var y = 2.192812;

            // Act
            var result = x.LessThan(y);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void LessThan_ShouldReturnFalse_WithSmallerPrecision()
        {
            // Arrange
            var x = 2.192812;
            var y = 2.19271;

            // Act
            var result = x.LessThan(y, .01);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void LessThan_ShouldReturnTrue_WithBiggerPrecision()
        {
            // Arrange
            var x = 2.192812;
            var y = 2.192813;

            // Act
            var result = x.LessThan(y, .00001);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void LessThan_ShouldReturnTrue_WithDefaultPrecision()
        {
            // Arrange
            var x = 2.192902;
            var y = 2.192912;

            // Act
            var result = x.LessThan(y);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void LessThan_ShouldReturnTrue_WithSmallerPrecision()
        {
            // Arrange
            var x = 2.192712;
            var y = 2.19281;

            // Act
            var result = x.LessThan(y, .01);

            // Assert
            Assert.IsTrue(result);
        }
    }
}