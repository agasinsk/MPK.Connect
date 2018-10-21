using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Builders;
using System;
using System.Linq;

namespace MPK.Connect.Test.Service.Builders
{
    [TestClass]
    public class FeedInfoBuilderTests
    {
        private FeedInfoBuilder _builder;

        [TestMethod]
        public void TestBuild()
        {
            // Arrange
            var dataString = "\"UM Wrocław\",\"http://www.wroclaw.pl/urzad\",\"pl\",\"20180611\",\"20180624\"";

            // Act
            var result = _builder.Build(dataString);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("UM Wrocław", result.Id);
            Assert.AreEqual("UM Wrocław", result.PublisherName);
            Assert.AreEqual("http://www.wroclaw.pl/urzad", result.PublisherUrl);
            Assert.AreEqual("pl", result.Language);
            Assert.AreEqual(new DateTime(2018, 06, 11), result.StartDate);
            Assert.AreEqual(new DateTime(2018, 06, 24), result.EndDate);
            Assert.IsNull(result.Version);
            Assert.IsTrue(result.GetRequiredProperties().All(p => p != null));
        }

        [TestInitialize]
        public void SetUp()
        {
            _builder = new FeedInfoBuilder();
            _builder.ReadEntityMappings("feed_publisher_name,feed_publisher_url,feed_lang,feed_start_date,feed_end_date");
        }
    }
}