using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Model.Enums;
using MPK.Connect.Service.Builders;
using System.Linq;

namespace MPK.Connect.Test.Service.Builders
{
    [TestClass]
    public class TransferBuilderTests
    {
        private TransferBuilder _builder;

        [TestMethod]
        public void TestBuild()
        {
            // Arrange
            var dataString = "S6,S7,2,300";

            // Act
            var result = _builder.Build(dataString);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("S6", result.FromStopId);
            Assert.AreEqual("S7", result.ToStopId);
            Assert.AreEqual(TransferTypes.Possible, result.TransferType);
            Assert.AreEqual(300, result.MinTransferTime);
            Assert.IsTrue(result.GetRequiredProperties().All(p => p != null));
        }

        [TestInitialize]
        public void SetUp()
        {
            _builder = new TransferBuilder();
            _builder.ReadEntityMappings("from_stop_id,to_stop_id,transfer_type,min_transfer_time");
        }
    }
}