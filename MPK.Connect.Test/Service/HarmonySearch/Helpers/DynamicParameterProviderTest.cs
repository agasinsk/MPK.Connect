using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;

namespace MPK.Connect.Test.Service.HarmonySearch.Helpers
{
    [TestClass]
    public class DynamicParameterProviderTest
    {
        private readonly DynamicParameterProvider _dynamicParameterProvider;

        public DynamicParameterProviderTest()
        {
            _dynamicParameterProvider = new DynamicParameterProvider(5);
        }

        [TestMethod]
        public void MarkCurrentParametersAsWinning_ShouldMoveTheParameterToWinningSet()
        {
            // Arrange
            var countBeforeMarking = _dynamicParameterProvider.ParameterSets.Count;
            var winningCountBeforeMarking = _dynamicParameterProvider.WinningParameterSets.Count;
            var parameterSet = _dynamicParameterProvider.GetParameterSet();

            // Act
            _dynamicParameterProvider.MarkCurrentParametersAsWinning();

            // Assert
            Assert.AreEqual(countBeforeMarking, _dynamicParameterProvider.ParameterSets.Count);
            Assert.AreEqual(winningCountBeforeMarking + 1, _dynamicParameterProvider.WinningParameterSets.Count);
            Assert.IsTrue(_dynamicParameterProvider.WinningParameterSets.Contains(parameterSet));
        }
    }
}