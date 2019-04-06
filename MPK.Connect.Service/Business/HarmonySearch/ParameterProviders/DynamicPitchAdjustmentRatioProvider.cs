using MPK.Connect.Service.Business.HarmonySearch.Core;

namespace MPK.Connect.Service.Business.HarmonySearch.ParameterProviders
{
    /// <summary>
    /// Dynamic pitch adjustment ratio provider
    /// </summary>
    public class DynamicPitchAdjustmentRatioProvider : IParameterProvider
    {
        private readonly long _maxImprovisationCount;

        private double _currentIterationCount;
        public double HarmonyMemoryConsiderationRatio { get; set; }
        public HarmonySearchType HarmonySearchType => HarmonySearchType.Improved;
        public double MaxPitchAdjustmentRatio { get; }

        public double MinPitchAdjustmentRatio { get; }
        public double PitchAdjustmentRatio => GetPitchAdjustmentRatio();

        public DynamicPitchAdjustmentRatioProvider(double harmonyMemoryConsiderationRatio, double maxPitchAdjustmentRatio, double minPitchAdjustmentRatio, long maxImprovisationCount)
        {
            HarmonyMemoryConsiderationRatio = harmonyMemoryConsiderationRatio;

            MaxPitchAdjustmentRatio = maxPitchAdjustmentRatio;
            MinPitchAdjustmentRatio = minPitchAdjustmentRatio;
            _maxImprovisationCount = maxImprovisationCount;

            _currentIterationCount = -1;
        }

        public double GetPitchAdjustmentRatio()
        {
            _currentIterationCount++;

            return MaxPitchAdjustmentRatio - (MaxPitchAdjustmentRatio - MinPitchAdjustmentRatio) * _currentIterationCount / _maxImprovisationCount;
        }
    }
}