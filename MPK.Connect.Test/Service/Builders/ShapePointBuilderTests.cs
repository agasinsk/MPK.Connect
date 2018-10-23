using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Builders;
using System.Linq;

namespace MPK.Connect.Test.Service.Builders
{
    [TestClass]
    public class ShapePointBuilderTests
    {
        private ShapeBuilder _builder;

        [TestMethod]
        public void TestBuild()
        {
            // Arrange
            var dataString = "683031,51.108199729672,17.021629047812,564";

            // Act
            var result = _builder.Build(dataString);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("683031", result.ShapeId);
            Assert.AreEqual(51.108199729672, result.PointLatitude);
            Assert.AreEqual(17.021629047812, result.PointLongitude);
            Assert.AreEqual(564, result.PointSequence);
            Assert.IsNull(result.DistTraveled);
            Assert.IsTrue(result.GetRequiredProperties().All(p => p != null));
        }

        [TestInitialize]
        public void SetUp()
        {
            _builder = new ShapeBuilder();
            _builder.ReadEntityMappings("shape_id,shape_pt_lat,shape_pt_lon,shape_pt_sequence");
        }
    }
}