using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Helpers;

namespace MPK.Connect.Test.Service
{
    [TestClass]
    public class EntityDataTests
    {
        private string[] _strings;
        private EntityData _entityData;

        [TestMethod]
        public void TestReturnsExistingValue()
        {
            // Arrange
            var index = 0;

            // Act
            var result = _entityData[index];

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result, _strings[index]);
        }

        [TestMethod]
        public void TestReturnUnderBoundsValue()
        {
            // Arrange
            var index = -1;

            // Act
            var result = _entityData[index];

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestReturnsOverBoundsValue()
        {
            // Arrange
            var index = _strings.Length + 1;

            // Act
            var result = _entityData[index];

            // Assert
            Assert.IsNull(result);
        }

        [TestInitialize]
        public void SetUp()
        {
            _strings = new[] { "1", "Agency", "Wro", "281812" };
            _entityData = new EntityData(_strings);
        }
    }
}