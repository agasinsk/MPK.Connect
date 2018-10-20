using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Helpers;

namespace MPK.Connect.Test.Service
{
    [TestClass]
    public class MappingsDictionaryTests
    {
        private MappingsDictionary _mappingsDictionary;

        [TestMethod]
        public void TestIndexOperatorReturnsExistingValue()
        {
            // Arrange
            var key = "id";
            var value = 1;
            _mappingsDictionary.Add(key, value);

            // Act
            var result = _mappingsDictionary[key];

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result, value);
        }

        [TestMethod]
        public void TestIndexOperatorReturnsDefaultValue()
        {
            // Arrange
            var key = "id";
            _mappingsDictionary.Clear();

            // Act
            var result = _mappingsDictionary[key];

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result, -1);
        }

        [TestInitialize]
        public void SetUp()
        {
            _mappingsDictionary = new MappingsDictionary();
        }
    }
}