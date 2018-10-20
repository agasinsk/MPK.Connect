using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Builders;
using System;

namespace MPK.Connect.Test.Service.Builders
{
    [TestClass]
    public class CalendarBuilderTests
    {
        private CalendarBuilder _calendarBuilder;

        [TestMethod]
        public void TestBuild()
        {
            // Arrange
            var dataString =
                "6,1,1,1,1,0,0,0,20180611,20180624";

            // Act
            var result = _calendarBuilder.Build(dataString);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("6", result.Id);
            Assert.AreEqual("6", result.ServiceId);
            Assert.IsTrue(result.Monday);
            Assert.IsTrue(result.Tuesday);
            Assert.IsTrue(result.Wednesday);
            Assert.IsTrue(result.Thursday);
            Assert.IsFalse(result.Friday);
            Assert.IsFalse(result.Saturday);
            Assert.IsFalse(result.Sunday);
            Assert.AreEqual(new DateTime(2018, 6, 11).Date, result.StartDate.Date);
            Assert.AreEqual(new DateTime(2018, 6, 24).Date, result.EndDate.Date);
        }

        [TestInitialize]
        public void SetUp()
        {
            _calendarBuilder = new CalendarBuilder();
            _calendarBuilder.ReadEntityMappings(
                "service_id,monday,tuesday,wednesday,thursday,friday,saturday,sunday,start_date,end_date");
        }
    }
}