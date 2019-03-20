using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;

namespace MPK.Connect.Service.Business.HarmonySearch.Core
{
    /// <summary>
    /// Implements improved harmony search algorithm
    /// </summary>
    public class ImprovedHarmonySearcher<T> : HarmonySearcher<T>, IImprovedHarmonySearcher<T>
    {
        private readonly DynamicPitchAdjustmentRatioProvider _dynamicPitchAdjustmentRatioProvider;
        public double MaxPitchAdjustmentRatio { get; set; }
        public double MinPitchAdjustmentRatio { get; set; }
        public override HarmonySearchType Type => HarmonySearchType.Improved;

        public ImprovedHarmonySearcher(IObjectiveFunction<T> function, int harmonyMemorySize,
            long maxImprovisationCount, double harmonyMemoryConsiderationRatio, double minPitchAdjustmentRatio,
            double maxPitchAdjustmentRatio) : base(function, harmonyMemorySize, maxImprovisationCount,
            harmonyMemoryConsiderationRatio)
        {
            _dynamicPitchAdjustmentRatioProvider = new DynamicPitchAdjustmentRatioProvider(maxPitchAdjustmentRatio, minPitchAdjustmentRatio, maxImprovisationCount);
        }

        /// <inheritdoc/>
        /// <summary>
        /// Looks for optimal solution of a function
        /// </summary>
        public override Harmony<T> SearchForHarmony()
        {
            InitializeHarmonyMemory();

            ImprovisationCount = 0;
            while (SearchingShouldContinue())
            {
                var worstHarmony = HarmonyMemory.WorstHarmony;

                HarmonyGenerator.PitchAdjustmentRatio = _dynamicPitchAdjustmentRatioProvider.GetPitchAdjustmentRatio(ImprovisationCount);

                var improvisedHarmony = HarmonyGenerator.ImproviseHarmony();
                if (improvisedHarmony.IsBetterThan(worstHarmony) && !HarmonyMemory.Contains(improvisedHarmony))
                {
                    HarmonyMemory.SwapWithWorstHarmony(improvisedHarmony);
                }
                ImprovisationCount++;
            }

            return HarmonyMemory.BestHarmony;
        }
    }
}