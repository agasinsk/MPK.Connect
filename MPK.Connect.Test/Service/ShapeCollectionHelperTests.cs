using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Model;
using MPK.Connect.Service.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace MPK.Connect.Test.Service
{
    [TestClass]
    public class ShapeCollectionHelperTests
    {
        private IShapeCollectionHelper _shapeCollectionHelper;
        private List<Shape> _testShapes;

        [TestMethod]
        public void TestGroupByShapeId()
        {
            // Arrange

            // Act
            var result = _shapeCollectionHelper.GroupByShapeId(_testShapes);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEqual(new List<int> { 1, 2, 3, 1, 2, 1, 2, 3 }, result.SelectMany(s => s.Value.Select(l => l.PointSequence)).ToList());
            CollectionAssert.AreEquivalent(new List<string> { "1", "2", "3" }, result.Select(s => s.Key.Id).ToList());
            CollectionAssert.AreEqual(new List<int> { 3, 2, 3 }, result.Select(s => s.Value.Count).ToList());
        }

        [TestMethod]
        public void TestGetShapeBases()
        {
            // Arrange

            // Act
            var result = _shapeCollectionHelper.GetShapeBases(_testShapes);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEquivalent(new List<string> { "1", "2", "3" }, result.Select(s => s.Id).ToList());
        }

        [TestMethod]
        public void TestGetShapeBasesByGrouping()
        {
            // Arrange

            // Act
            var result = _shapeCollectionHelper.GetShapeBasesByGrouping(_testShapes);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEquivalent(new List<string> { "1", "2", "3" }, result.Select(s => s.Id).ToList());
        }

        [TestInitialize]
        public void SetUp()
        {
            _shapeCollectionHelper = new ShapeCollectionHelper();
            _testShapes = new List<Shape>
            {
                new Shape
                {
                    ShapeId = "1",
                    PointSequence = 1,
                },
                new Shape
                {
                    ShapeId = "1",
                    PointSequence = 3,
                },
                new Shape
                {
                    ShapeId = "1",
                    PointSequence = 2,
                },
                new Shape
                {
                    ShapeId = "2",
                    PointSequence = 1,
                },
                new Shape
                {
                    ShapeId = "2",
                    PointSequence = 2,
                },
                new Shape
                {
                    ShapeId = "3",
                    PointSequence = 1,
                },
                new Shape
                {
                    ShapeId = "3",
                    PointSequence = 3,
                },
                new Shape
                {
                    ShapeId = "3",
                    PointSequence = 2,
                }
            };
        }
    }
}