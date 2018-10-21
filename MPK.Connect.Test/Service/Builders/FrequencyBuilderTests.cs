using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Model.Enums;
using MPK.Connect.Service.Builders;
using System;
using System.Linq;

namespace MPK.Connect.Test.Service.Builders
{
    [TestClass]
    public class FrequencyBuilderTests
    {
        private FrequencyBuilder _builder;

        [TestMethod]
        public void TestBuild()
        {
            // Arrange
            var dataString = "AWE1,05:30:00,06:30:00,300\r\n";

            // Act
            var result = _builder.Build(dataString);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("AWE1", result.TripId);
            Assert.AreEqual(new DateTime(1, 1, 1, 5, 30, 0), result.StartTime);
            Assert.AreEqual(new DateTime(1, 1, 1, 6, 30, 0), result.EndTime);

            Assert.AreEqual(300, result.HeadwaySecs);
            Assert.AreEqual(ExactTimes.NotExactlyScheduled, result.ExactTimes);
            Assert.IsTrue(result.GetRequiredProperties().All(p => p != null));
        }

        [TestInitialize]
        public void SetUp()
        {
            _builder = new FrequencyBuilder();
            _builder.ReadEntityMappings("trip_id,start_time,end_time,headway_secs");
        }
    }
}