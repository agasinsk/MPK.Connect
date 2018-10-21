using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Model.Enums;
using MPK.Connect.Service.Builders;
using System;
using System.Linq;

namespace MPK.Connect.Test.Service.Builders
{
    [TestClass]
    public class StopTimeBuilderTests
    {
        private StopTimeBuilder _builder;

        [TestMethod]
        public void TestBuild()
        {
            // Arrange
            var dataString = "3_6045313,08:11:00,08:11:00,1684,0,0,1";

            // Act
            var result = _builder.Build(dataString);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("3_6045313", result.TripId);
            Assert.AreEqual(new DateTime(1, 1, 1, 8, 11, 0), result.ArrivalTime);
            Assert.AreEqual(new DateTime(1, 1, 1, 8, 11, 0), result.DepartureTime);
            Assert.AreEqual("1684", result.StopId);
            Assert.AreEqual(0, result.StopSequence);
            Assert.AreEqual(PickupTypes.Regular, result.PickupType);
            Assert.AreEqual(DropOffTypes.NoDropOff, result.DropOffTypes);

            Assert.IsTrue(result.GetRequiredProperties().All(p => p != null));
        }

        [TestInitialize]
        public void SetUp()
        {
            _builder = new StopTimeBuilder();
            _builder.ReadEntityMappings("trip_id,arrival_time,departure_time,stop_id,stop_sequence,pickup_type,drop_off_type");
        }
    }
}