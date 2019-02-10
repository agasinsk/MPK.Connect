using Microsoft.VisualStudio.TestTools.UnitTesting;
using TspLibNet;

namespace MPK.Connect.Test.Service.HarmonySearch.Helpers
{
    [Ignore]
    [TestClass]
    public class TravelingSalesmanObjectiveFunctionTest
    {
        [TestMethod]
        public void TestIfProblemIsLoadedWithDefaultName()
        {
            // Arrange
            var expectedComment = "52 locations in Berlin (Groetschel)";
            var expectedNodeCount = 52;

            // Act
            var result = new TravelingSalesmanObjectiveFunction();
            var problemItem = result.ProblemItem;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(problemItem);

            Assert.AreEqual(TravelingSalesmanObjectiveFunction.DefaultProblemName, problemItem.Problem.Name);
            Assert.AreEqual(expectedComment, problemItem.Problem.Comment);
            Assert.AreEqual(expectedNodeCount, problemItem.Problem.NodeProvider.CountNodes());
            Assert.AreEqual(expectedNodeCount, problemItem.Problem.NodeProvider.CountNodes());
            Assert.AreEqual(ProblemType.TSP, problemItem.Problem.Type);
        }

        [TestMethod]
        public void TestIfProblemIsLoadedWithNonDefaultName()
        {
            // Arrange
            var problemName = "brazil58";
            var expectedComment = "58 cities in Brazil (Ferreira)";
            var expectedNodeCount = 58;

            // Act
            var result = new TravelingSalesmanObjectiveFunction(problemName);
            var problemItem = result.ProblemItem;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(problemItem);

            Assert.AreEqual(problemName, problemItem.Problem.Name);
            Assert.AreEqual(expectedComment, problemItem.Problem.Comment);
            Assert.AreEqual(expectedNodeCount, problemItem.Problem.NodeProvider.CountNodes());
            Assert.AreEqual(ProblemType.TSP, problemItem.Problem.Type);
        }
    }
}