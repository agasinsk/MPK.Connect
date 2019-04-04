using MPK.Connect.Service.Business.HarmonySearch.Functions;

namespace MPK.Connect.Service.Business.HarmonySearch.Core
{
    public class AntColonyHarmonySearcher<T> : HarmonySearcher<T>
    {
        public AntColonyHarmonySearcher(IAntColonyObjectiveFunction<T> function) : base(function)
        {
        }

        public AntColonyHarmonySearcher(IAntColonyObjectiveFunction<T> function, int harmonyMemorySize, long maxImprovisationCount) : base(function, harmonyMemorySize, maxImprovisationCount)
        {
        }

        public AntColonyHarmonySearcher(IAntColonyObjectiveFunction<T> function, int harmonyMemorySize, long maxImprovisationCount, double harmonyMemoryConsiderationRatio) : base(function, harmonyMemorySize, maxImprovisationCount, harmonyMemoryConsiderationRatio)
        {
        }

        public AntColonyHarmonySearcher(IAntColonyObjectiveFunction<T> function, int harmonyMemorySize, long maxImprovisationCount, double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio) : base(function, harmonyMemorySize, maxImprovisationCount, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio)
        {
        }

        public override Harmony<T> SearchForHarmony()
        {
            InitializeHarmonyMemory();

            ImprovisationCount = 0;
            while (SearchingShouldContinue())
            {
                var worstHarmony = HarmonyMemory.WorstHarmony;

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