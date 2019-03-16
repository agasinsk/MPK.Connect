namespace MPK.Connect.Service.Business.HarmonySearch.Helpers
{
    public class DynamicPitchAdjustmentRatioProvider
    {
        private readonly long _maxImprovisationCount;

        private readonly double _maxPitchAdjustmentRatio;

        private readonly double _minPitchAdjustmentRatio;

        public DynamicPitchAdjustmentRatioProvider(double maxPitchAdjustmentRatio, double minPitchAdjustmentRatio, long maxImprovisationCount)
        {
            _maxPitchAdjustmentRatio = maxPitchAdjustmentRatio;
            _minPitchAdjustmentRatio = minPitchAdjustmentRatio;
            _maxImprovisationCount = maxImprovisationCount;
        }

        public double GetPitchAdjustmentRatio(int iterationCount)
        {
            return _maxPitchAdjustmentRatio - (_maxPitchAdjustmentRatio - _minPitchAdjustmentRatio) * iterationCount / _maxImprovisationCount;
        }
    }
}