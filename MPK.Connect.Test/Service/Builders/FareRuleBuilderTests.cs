using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Builders;
using System.Linq;

namespace MPK.Connect.Test.Service.Builders
{
    [TestClass]
    public class FareRuleBuilderTests
    {
        private FareRuleBuilder _builder;

        [TestMethod]
        public void TestBuild()
        {
            // Arrange
            var dataString = "a,TSW,1,1,";

            // Act
            var result = _builder.Build(dataString);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("a", result.FareId);
            Assert.AreEqual("TSW", result.RouteId);
            Assert.AreEqual("1", result.OriginId);
            Assert.AreEqual("1", result.DestinationId);
            Assert.AreEqual(string.Empty, result.ContainsId);

            Assert.IsTrue(result.GetRequiredProperties().All(p => p != null));
        }

        [TestInitialize]
        public void SetUp()
        {
            _builder = new FareRuleBuilder();
            _builder.ReadEntityMappings("fare_id,route_id,origin_id,destination_id,contains_id");
        }
    }
}