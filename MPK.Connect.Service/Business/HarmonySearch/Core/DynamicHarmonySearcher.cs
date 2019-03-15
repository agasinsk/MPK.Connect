using MPK.Connect.Service.Business.HarmonySearch.Functions;

namespace MPK.Connect.Service.Business.HarmonySearch.Core
{
    public class DynamicHarmonySearcher<T> : HarmonySearcher<T>
    {
        public override HarmonySearchType Type => HarmonySearchType.Dynamic;

        public DynamicHarmonySearcher(IObjectiveFunction<T> function) : base(function)
        {
        }

        public DynamicHarmonySearcher(IObjectiveFunction<T> function, int harmonyMemorySize, long maxImprovisationCount, double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio, bool shouldImprovePitchAdjustingScenario, double minPitchAdjustmentRatio, double maxPitchAdjustmentRatio) : base(function, harmonyMemorySize, maxImprovisationCount, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio, shouldImprovePitchAdjustingScenario, minPitchAdjustmentRatio, maxPitchAdjustmentRatio)
        {
        }

        public override Harmony<T> SearchForHarmony()
        {
            InitializeHarmonyMemory();

            for (var improvisationCount = 0; improvisationCount < MaxImprovisationCount; improvisationCount++)
            {
                var worstHarmony = HarmonyMemory.WorstHarmony;
                if (ShouldImprovePitchAdjustingScenario)
                {
                    HarmonyGenerator.PitchAdjustmentRatio = GetCurrentPitchAdjustingRatio(improvisationCount);
                }

                var improvisedHarmony = HarmonyGenerator.ImproviseHarmony();
                if (improvisedHarmony.IsBetterThan(worstHarmony) && !HarmonyMemory.Contains(improvisedHarmony))
                {
                    HarmonyMemory.SwapWithWorstHarmony(improvisedHarmony);
                    HarmonyGenerator.MarkCurrentParametersAsWinning();
                }
            }

            return HarmonyMemory.BestHarmony;
        }
    }
}