using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;
using static MPK.Connect.Service.Business.HarmonySearch.Constants.HarmonySearchConstants;

namespace MPK.Connect.Test.Service.HarmonySearch.Helpers
{
    [TestClass]
    public class PitchAdjustmentRatioProviderTest
    {
        private readonly DynamicPitchAdjustmentRatioProvider _parProvider;

        public PitchAdjustmentRatioProviderTest()
        {
            _parProvider = new DynamicPitchAdjustmentRatioProvider(DefaultMaxPitchAdjustmentRatio, DefaultMinPitchAdjustmentRatio, DefaultMaxImprovisationCount);
        }

        [TestMethod]
        public void GetPitchAdjustmentRatio_ReturnsCorrectValue()
        {
            // Arrange
            var iterationCount = 11;
            var expectedResult = DefaultMaxPitchAdjustmentRatio -
                                 (DefaultMaxPitchAdjustmentRatio - DefaultMinPitchAdjustmentRatio) * iterationCount /
                                 DefaultMaxImprovisationCount;

            // Act
            var result = _parProvider.GetPitchAdjustmentRatio(iterationCount);

            // assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void GetPitchAdjustmentRatio_ReturnsDefaultMaxValue_IfIterationIsZero()
        {
            // Arrange
            var iterationCount = 0;

            // Act
            var result = _parProvider.GetPitchAdjustmentRatio(iterationCount);

            // assert
            Assert.AreEqual(DefaultMaxPitchAdjustmentRatio, result);
        }
    }
}