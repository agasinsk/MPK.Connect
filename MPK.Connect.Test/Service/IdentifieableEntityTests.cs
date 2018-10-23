using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Model;
using MPK.Connect.Service.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace MPK.Connect.Test.Service
{
    [TestClass]
    public class IdentifiableEntityTests
    {
        [TestMethod]
        public void TestGetDistinctShapes()
        {
            // Arrange
            var entities = new List<Shape>
            {
                new Shape{Id = "1"},
                new Shape{Id = "2"},
                new Shape{Id = "3"},
                new Shape{Id = "3"},
                new Shape{Id = "2"},
            };

            // Act
            var result = entities.Distinct(new ShapeComparer()).ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEquivalent(new List<string> { "1", "2", "3" }, result.Select(e => e.Id).ToList());
        }

        [TestMethod]
        public void TestGetsDistinctElement()
        {
            // Arrange
            var entities = new List<Shape>
            {
                new Shape{Id = "1"},
                new Shape{Id = "2"},
                new Shape{Id = "3"},
                new Shape{Id = "3"},
                new Shape{Id = "2"},
            };

            // Act
            var result = entities.Distinct(new IdentifiableEntityComparer<string>()).ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEquivalent(new List<string> { "1", "2", "3" }, result.Select(e => e.Id).ToList());
        }

        [TestMethod]
        public void TestGetDistinctElementsUsingGroupBy()
        {
            // Arrange
            var entities = new List<Shape>
            {
                new Shape{Id = "1"},
                new Shape{Id = "2"},
                new Shape{Id = "3"},
                new Shape{Id = "3"},
                new Shape{Id = "2"},
                new Shape{Id = "3"},
            };

            // Act
            var result = entities.GroupBy(s => s.Id).Select(g => g.First()).ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEquivalent(new List<string> { "1", "2", "3" }, result.Select(e => e.Id).ToList());
        }

        [TestMethod]
        public void TestHasDistinctIdReturnsTrue()
        {
            // Arrange
            var entity = new Shape();

            // Act
            var result = entity.HasDistinctId();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestHasDistinctIdReturnsFalse()
        {
            // Arrange
            var entity = new FeedInfo();

            // Act
            var result = entity.HasDistinctId();

            // Assert
            Assert.IsFalse(result);
        }

        [TestInitialize]
        public void SetUp()
        {
        }
    }
}