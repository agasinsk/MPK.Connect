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

        [TestInitialize]
        public void SetUp()
        {
            _builder = new StopTimeBuilder();
            _builder.ReadEntityMappings("trip_id,arrival_time,departure_time,stop_id,stop_sequence,pickup_type,drop_off_type");
        }

        [TestMethod]
        public void TestBuild()
        {
            // Arrange
            var dataString = "3_6357151,20:55:00,20:55:00,1038,12,0,0";

            // Act
            var result = _builder.Build(dataString);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("3_6357151", result.TripId);
            Assert.AreEqual(new TimeSpan(0, 20, 55, 0), result.ArrivalTime);
            Assert.AreEqual(new TimeSpan(0, 20, 55, 0), result.DepartureTime);
            Assert.AreEqual("1038", result.StopId);
            Assert.AreEqual(12, result.StopSequence);
            Assert.AreEqual(PickupTypes.Regular, result.PickupType);
            Assert.AreEqual(DropOffTypes.Regular, result.DropOffTypes);

            Assert.IsTrue(result.GetRequiredProperties().All(p => p != null));
        }
    }
}