using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Model.Enums;
using MPK.Connect.Service.Builders;
using System.Linq;

namespace MPK.Connect.Test.Service.Builders
{
    [TestClass]
    public class FareAttributeBuilderTests
    {
        private FareAttributeBuilder _builder;

        [TestMethod]
        public void TestBuild()
        {
            // Arrange
            var dataString = "1,0.00,USD,0,0,0";

            // Act
            var result = _builder.Build(dataString);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("1", result.FareId);
            Assert.AreEqual(0.00, result.Price);
            Assert.AreEqual("USD", result.CurrencyType);
            Assert.AreEqual(PaymentMethods.OnBoard, result.PaymentMethod);
            Assert.AreEqual(0, result.Transfers);
            Assert.AreEqual(0, result.TransferDuration);
            Assert.IsTrue(result.GetRequiredProperties().All(p => p != null));
        }

        [TestInitialize]
        public void SetUp()
        {
            _builder = new FareAttributeBuilder();
            _builder.ReadEntityMappings("fare_id,price,currency_type,payment_method,transfers,transfer_duration");
        }
    }
}