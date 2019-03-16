using MPK.Connect.Service.Business.HarmonySearch.Functions;

namespace MPK.Connect.Service.Business.HarmonySearch.Core
{
    public class DynamicHarmonySearcher<T> : HarmonySearcher<T>
    {
        public override HarmonySearchType Type => HarmonySearchType.Dynamic;

        public DynamicHarmonySearcher(IGeneralObjectiveFunction<T> function, int harmonyMemorySize, long maxImprovisationCount) : base(function, harmonyMemorySize, maxImprovisationCount)
        {
        }

        public override Harmony<T> SearchForHarmony()
        {
            InitializeHarmonyMemory();

            for (var improvisationCount = 0; improvisationCount < MaxImprovisationCount; improvisationCount++)
            {
                var worstHarmony = HarmonyMemory.WorstHarmony;

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