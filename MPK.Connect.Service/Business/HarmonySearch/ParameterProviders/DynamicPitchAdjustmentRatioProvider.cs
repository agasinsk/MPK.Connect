using MPK.Connect.Service.Business.HarmonySearch.Core;

namespace MPK.Connect.Service.Business.HarmonySearch.ParameterProviders
{
    /// <summary>
    /// Dynamic pitch adjustment ratio provider
    /// </summary>
    public class DynamicPitchAdjustmentRatioProvider : IParameterProvider
    {
        private readonly long _maxImprovisationCount;

        private readonly double _maxPitchAdjustmentRatio;

        private readonly double _minPitchAdjustmentRatio;
        private double _currentIterationCount;

        public double HarmonyMemoryConsiderationRatio { get; set; }

        public HarmonySearchType HarmonySearchType => HarmonySearchType.Improved;
        public double PitchAdjustmentRatio => GetPitchAdjustmentRatio();

        public DynamicPitchAdjustmentRatioProvider(double harmonyMemoryConsiderationRatio, double maxPitchAdjustmentRatio, double minPitchAdjustmentRatio, long maxImprovisationCount)
        {
            HarmonyMemoryConsiderationRatio = harmonyMemoryConsiderationRatio;

            _maxPitchAdjustmentRatio = maxPitchAdjustmentRatio;
            _minPitchAdjustmentRatio = minPitchAdjustmentRatio;
            _maxImprovisationCount = maxImprovisationCount;

            _currentIterationCount = -1;
        }

        public double GetPitchAdjustmentRatio()
        {
            _currentIterationCount++;

            return _maxPitchAdjustmentRatio - (_maxPitchAdjustmentRatio - _minPitchAdjustmentRatio) * _currentIterationCount / _maxImprovisationCount;
        }
    }
}