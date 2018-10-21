using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Builders;
using System.Linq;

namespace MPK.Connect.Test.Service.Builders
{
    [TestClass]
    public class TripBuilderTests
    {
        private TripBuilder _builder;

        [TestMethod]
        public void TestBuild()
        {
            // Arrange
            var dataString = "1,3,3_6045313,\"POŚWIĘTNE\",1,683018,1,28,683018";

            // Act
            var result = _builder.Build(dataString);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("1", result.RouteId);
            Assert.AreEqual("3", result.ServiceId);
            Assert.AreEqual("3_6045313", result.Id);
            Assert.AreEqual("POŚWIĘTNE", result.HeadSign);
            Assert.AreEqual(1, result.DirectionId);
            Assert.AreEqual("683018", result.ShapeId);

            Assert.IsTrue(result.GetRequiredProperties().All(p => p != null));
        }

        [TestInitialize]
        public void SetUp()
        {
            _builder = new TripBuilder();
            _builder.ReadEntityMappings("route_id,service_id,trip_id,trip_headsign,direction_id,shape_id,brigade_id,vehicle_id,variant_id");
        }
    }
}