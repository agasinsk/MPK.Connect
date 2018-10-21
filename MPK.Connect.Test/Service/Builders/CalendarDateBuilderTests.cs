using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Model.Enums;
using MPK.Connect.Service.Builders;
using System;
using System.Linq;

namespace MPK.Connect.Test.Service.Builders
{
    [TestClass]
    public class CalendarDateBuilderTests
    {
        private CalendarDateBuilder _calendarDateBuilder;

        [TestMethod]
        public void TestBuild()
        {
            // Arrange
            var dataString = "6,20180611,1";

            // Act
            var result = _calendarDateBuilder.Build(dataString);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("6", result.Id);
            Assert.AreEqual("6", result.ServiceId);
            Assert.AreEqual(new DateTime(2018, 6, 11).Date, result.Date);
            Assert.AreEqual(ExceptionRules.Added, result.ExceptionRule);
            Assert.IsTrue(result.GetRequiredProperties().All(p => p != null));
        }

        [TestInitialize]
        public void SetUp()
        {
            _calendarDateBuilder = new CalendarDateBuilder();
            _calendarDateBuilder.ReadEntityMappings("service_id, date,exception_type");
        }
    }
}