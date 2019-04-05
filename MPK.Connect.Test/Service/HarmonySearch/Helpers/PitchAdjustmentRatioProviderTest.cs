using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Business.HarmonySearch.ParameterProviders;
using static MPK.Connect.Service.Business.HarmonySearch.Constants.HarmonySearchConstants;

namespace MPK.Connect.Test.Service.HarmonySearch.Helpers
{
    [TestClass]
    public class PitchAdjustmentRatioProviderTest
    {
        private const double HarmonyMemoryConsiderationRatio = 0.9;
        private readonly DynamicPitchAdjustmentRatioProvider _parProvider;

        public PitchAdjustmentRatioProviderTest()
        {
            _parProvider = new DynamicPitchAdjustmentRatioProvider(HarmonyMemoryConsiderationRatio, DefaultMaxPitchAdjustmentRatio, DefaultMinPitchAdjustmentRatio, DefaultMaxImprovisationCount);
        }

        [TestMethod]
        public void GetHarmonyMemoryConsiderationRatio_ReturnsCorrectValue()
        {
            // Arrange
            _parProvider.GetPitchAdjustmentRatio();
            _parProvider.GetPitchAdjustmentRatio();

            // Act
            var result = _parProvider.HarmonyMemoryConsiderationRatio;

            // assert
            Assert.AreEqual(HarmonyMemoryConsiderationRatio, result);
        }

        [TestMethod]
        public void GetPitchAdjustmentRatio_ReturnsCorrectValue()
        {
            // Arrange
            var iterationCount = 6;

            for (int i = 0; i < iterationCount; i++)
            {
                _parProvider.GetPitchAdjustmentRatio();
            }

            var expectedResult = DefaultMaxPitchAdjustmentRatio -
                                 (DefaultMaxPitchAdjustmentRatio - DefaultMinPitchAdjustmentRatio) * iterationCount /
                                 DefaultMaxImprovisationCount;

            // Act
            var result = _parProvider.GetPitchAdjustmentRatio();

            // assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void GetPitchAdjustmentRatio_ReturnsDefaultMaxValue_IfIterationIsZero()
        {
            // Arrange

            // Act
            var result = _parProvider.GetPitchAdjustmentRatio();

            // assert
            Assert.AreEqual(DefaultMaxPitchAdjustmentRatio, result);
        }
    }
}