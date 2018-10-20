using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Builders;

namespace MPK.Connect.Test.Service.Builders
{
    [TestClass]
    public class AgencyBuilderTests
    {
        private AgencyBuilder _agencyBuilder;

        [TestMethod]
        public void TestBuild()
        {
            // Arrange
            var agencyString =
                "2,\"MPK Autobusy\",\"http://www.mpk.wroc.pl\",\"Europe/Warsaw\",\"71 321 72 71\",\"pl\"";

            // Act
            var result = _agencyBuilder.Build(agencyString);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("2", result.Id);
            Assert.AreEqual("MPK Autobusy", result.Name);
            Assert.AreEqual("http://www.mpk.wroc.pl", result.Url);
            Assert.AreEqual("Europe/Warsaw", result.Timezone);
            Assert.AreEqual("71 321 72 71", result.Phone);
            Assert.AreEqual("pl", result.Language);
            Assert.IsNull(result.FareUrl);
            Assert.IsNull(result.Email);
        }

        [TestInitialize]
        public void SetUp()
        {
            _agencyBuilder = new AgencyBuilder();
            _agencyBuilder.ReadEntityMappings(
                "agency_id,agency_name,agency_url,agency_timezone,agency_phone,agency_lang");
        }
    }
}