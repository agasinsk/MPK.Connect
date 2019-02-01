using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Builders;

namespace MPK.Connect.Test.Service.Builders
{
    [TestClass]
    public class StopBuilderTests
    {
        private StopBuilder _builder;

        [TestInitialize]
        public void SetUp()
        {
            _builder = new StopBuilder();
            _builder.ReadEntityMappings("stop_id,stop_code,stop_name,stop_lat,stop_lon");
        }

        [TestMethod]
        public void TestBuild()
        {
            // Arrange
            var dataString = "80,18372,\"Wolska\",51.1469110000,16.8693790000";

            // Act
            var result = _builder.Build(dataString);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(80, result.Id);
            Assert.AreEqual("18372", result.Code);
            Assert.AreEqual("Wolska", result.Name);
            Assert.AreEqual(51.146911, result.Latitude);
            Assert.AreEqual(16.869379, result.Longitude);

            Assert.IsTrue(result.GetRequiredProperties().All(p => p != null));
        }
    }
}