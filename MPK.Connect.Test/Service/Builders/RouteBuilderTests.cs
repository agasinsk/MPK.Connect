using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Model.Enums;
using MPK.Connect.Service.Builders;
using System.Linq;

namespace MPK.Connect.Test.Service.Builders
{
    [TestClass]
    public class RouteBuilderTests
    {
        private RouteBuilder _builder;

        [TestMethod]
        public void TestBuild()
        {
            // Arrange
            var dataString = "1,3,\" 1\",\"\",\"POŚWIĘTNE - Żmigrodzka - Trzebnicka - pl. Powstańców Wielkopolskich - Słowiańska - Jedności Narodowej - Nowowiejska - Piastowska - Skłodowskiej-Curie - Wróblewskiego - Olszewskiego - BISKUPIN|BISKUPIN - Wróblewskiego - Skłodowskiej-Curie - Piastowska - Nowowiejska - Jedności Narodowej - Słowiańska - pl. Powstańców Wielkopolskich - Trzebnicka - Żmigrodzka - POŚWIĘTNE\",0,31,\"2018-04-21\",\"2999-01-01\"";

            // Act
            var result = _builder.Build(dataString);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("1", result.Id);
            Assert.AreEqual("3", result.AgencyId);
            Assert.AreEqual("1", result.ShortName);
            Assert.AreEqual("", result.LongName);
            Assert.AreEqual("POŚWIĘTNE - Żmigrodzka - Trzebnicka - pl. Powstańców Wielkopolskich - Słowiańska - Jedności Narodowej - Nowowiejska - Piastowska - Skłodowskiej-Curie - Wróblewskiego - Olszewskiego - BISKUPIN|BISKUPIN - Wróblewskiego - Skłodowskiej-Curie - Piastowska - Nowowiejska - Jedności Narodowej - Słowiańska - pl. Powstańców Wielkopolskich - Trzebnicka - Żmigrodzka - POŚWIĘTNE", result.Description);
            Assert.AreEqual(RouteTypes.Tram, result.Type);
            Assert.IsNull(result.Color);
            Assert.IsNull(result.SortOrder);
            Assert.IsNull(result.TextColor);
            Assert.IsNull(result.Url);
            Assert.IsTrue(result.GetRequiredProperties().All(p => p != null));
        }

        [TestInitialize]
        public void SetUp()
        {
            _builder = new RouteBuilder();
            _builder.ReadEntityMappings("route_id,agency_id,route_short_name,route_long_name,route_desc,route_type,route_type2_id,valid_from,valid_until");
        }
    }
}