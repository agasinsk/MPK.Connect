using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;

namespace MPK.Connect.Test.Service.Business
{
    [TestClass]
    public class GraphTests
    {
        private Graph<StopDto> _graph;

        [TestMethod]
        public void TestGraphStructure()
        {
            // Arrange
            var stops = new List<StopDto>
            {
                new StopDto{ Id = "1", Name = "A" },
                new StopDto{ Id = "2", Name = "B" },
                new StopDto{ Id = "3", Name = "C" },
                new StopDto{ Id = "4", Name = "D" },
                new StopDto{ Id = "5", Name = "E" },
            };
            _graph.AddNodes(stops);

            _graph.AddDirectedEdge(stops[0], stops[1], 2);
            _graph.AddDirectedEdge(stops[1], stops[2], 1);
            _graph.AddDirectedEdge(stops[2], stops[4], 3);
            _graph.AddDirectedEdge(stops[0], stops[4], 4);
            _graph.AddDirectedEdge(stops[4], stops[3], 1);
            _graph.AddDirectedEdge(stops[3], stops[0], 2);

            // Act
            var result = _graph.Nodes;
            var neighbors = (result.First() as GraphNode<StopDto>)?.Neighbors;

            // Assert
            Assert.AreEqual(stops.Count, result.Count);
            Assert.AreEqual(stops.First(), result.First().Value);
            Assert.AreEqual(2, neighbors?.Count);
        }

        [TestInitialize]
        public void SetUp()
        {
            _graph = new Graph<StopDto>();
        }

        [TestCleanup]
        public void TearDown()
        {
            _graph = null;
        }
    }
}