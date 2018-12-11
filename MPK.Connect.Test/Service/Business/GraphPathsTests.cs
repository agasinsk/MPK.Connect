using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.Graph;
using System.Collections.Generic;
using System.Linq;

namespace MPK.Connect.Test.Service.Business
{
    [TestClass]
    public class GraphPathsTests
    {
        private Graph<string, City> _graph;
        private Dictionary<string, City> cities;

        [TestInitialize]
        public void SetUp()
        {
            cities = new Dictionary<string, City>
            {
                {"S", new City("S") },
                {"A", new City("A") },
                {"B", new City("B") },
                {"C", new City("C") },
                {"D", new City("D") },
                {"E", new City("E") },
                {"G", new City("G") },
            };

            _graph = new Graph<string, City>(cities);

            _graph.AddEdge(cities["S"], cities["A"], 3);
            _graph.AddEdge(cities["S"], cities["B"], 5);
            _graph.AddEdge(cities["B"], cities["C"], 4);
            _graph.AddEdge(cities["C"], cities["E"], 6);
            _graph.AddEdge(cities["B"], cities["A"], 3);
            _graph.AddEdge(cities["A"], cities["D"], 3);
            _graph.AddEdge(cities["D"], cities["G"], 5);
        }

        [TestCleanup]
        public void TearDown()
        {
            _graph = null;
        }

        [TestMethod]
        public void TestAStar()
        {
            // Arrange
            var start = cities["S"];
            var dest = cities["G"];

            // Act
            var path = _graph.A_Star(start, dest);

            // Assert
            Assert.AreEqual(4, path.Count);
            Assert.AreEqual(11, path.Cost);
            CollectionAssert.AreEqual(new[] { "S", "A", "D", "G" }, path.Select(s => s.Id).ToArray());
        }

        [TestMethod]
        public void TestAStarReturnsCorrectlyWithBAsStart()
        {
            // Arrange
            var start = cities["B"];
            var dest = cities["G"];

            // Act
            var path = _graph.A_Star(start, dest);

            // Assert
            Assert.AreEqual(4, path.Count);
            Assert.AreEqual(11, path.Cost);
            CollectionAssert.AreEqual(new[] { "B", "A", "D", "G" }, path.Select(s => s.Id).ToArray());
        }

        [TestMethod]
        public void TestAStarReturnsCorrectlyWithEAsStart()
        {
            // Arrange
            var start = cities["E"];
            var dest = cities["G"];

            // Act
            var path = _graph.A_Star(start, dest);

            // Assert
            Assert.AreEqual(6, path.Count);
            Assert.AreEqual(21, path.Cost);
            CollectionAssert.AreEqual(new[] { "E", "C", "B", "A", "D", "G" }, path.Select(s => s.Id).ToArray());
        }
    }
}