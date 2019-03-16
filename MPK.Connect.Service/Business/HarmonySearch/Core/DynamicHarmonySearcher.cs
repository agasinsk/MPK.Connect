using MPK.Connect.Service.Business.HarmonySearch.Constants;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;

namespace MPK.Connect.Service.Business.HarmonySearch.Core
{
    public class DynamicHarmonySearcher<T> : HarmonySearcher<T>
    {
        private readonly DynamicParameterProvider _dynamicParameterProvider;

        public override HarmonySearchType Type => HarmonySearchType.Dynamic;

        public DynamicHarmonySearcher(IObjectiveFunction<T> function, int harmonyMemorySize, long maxImprovisationCount) : base(function, harmonyMemorySize, maxImprovisationCount)
        {
            _dynamicParameterProvider = new DynamicParameterProvider(HarmonySearchConstants.DefaultParameterListCapacity);
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