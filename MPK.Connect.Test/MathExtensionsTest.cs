using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Model;
using MPK.Connect.Service.Helpers;

namespace MPK.Connect.Test
{
    [TestClass]
    public class MathExtensionsTest
    {
        [TestMethod]
        public void Test45DegreesToRadians()
        {
            // Arrange
            var angle = 45d;

            // Act
            var result = angle.ToRadians();

            // Assert
            Assert.AreEqual(Math.PI / 4, result);
        }

        [TestMethod]
        public void Test90DegreesToRadians()
        {
            // Arrange
            var angle = 90d;

            // Act
            var result = angle.ToRadians();

            // Assert
            Assert.AreEqual(Math.PI / 2, result);
        }

        [TestMethod]
        public void Test180DegreesToRadians()
        {
            // Arrange
            var angle = 180d;

            // Act
            var result = angle.ToRadians();

            // Assert
            Assert.AreEqual(Math.PI, result);
        }

        [TestMethod]
        public void TestGetDistanceBetweenStops()
        {
            // Arrange
            // Gal. Dominikańska
            var source = new Stop
            {
                Latitude = 51.113144,
                Longitude = 17.006870
            };

            // pl. strzegomski
            var destination = new Stop
            {
                Latitude = 51.108334,
                Longitude = 17.038629
            };

            // Act
            var result = source.GetDistanceTo(destination);

            // Assert
            Assert.AreEqual(2.28, result, 0.01);
        }

        [TestMethod]
        public void TestGetDistanceBetweenStopsWithTheSameStop()
        {
            // Arrange
            var source = new Stop
            {
                Latitude = 51.113144,
                Longitude = 17.006870
            };

            // Act
            var result = source.GetDistanceTo(source);

            // Assert
            Assert.AreEqual(0, result);
        }
    }
}