using System;
using MPK.Connect.Service.Business.HarmonySearch.Constants;
using MPK.Connect.Service.Business.HarmonySearch.Generator;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;

namespace MPK.Connect.Service.Business.HarmonySearch.Core
{
    /// <summary>
    /// Implements harmony search with dynamically adjusted parameter values (ParameterSetList and WinningParameterSetList)
    /// </summary>
    /// <typeparam name="T">Type of entities</typeparam>
    public class DynamicHarmonySearcher<T> : HarmonySearcher<T>
    {
        private readonly DynamicParameterProvider _parameterProvider;
        public override HarmonySearchType Type => HarmonySearchType.Dynamic;

        public DynamicHarmonySearcher(IHarmonyGenerator<T> harmonyGenerator) : base(harmonyGenerator)
        {
            _parameterProvider = new DynamicParameterProvider(HarmonySearchConstants.DefaultParameterListCapacity);
        }

        public override Harmony<T> SearchForHarmony()
        {
            InitializeHarmonyMemory();

            for (var improvisationCount = 0; improvisationCount < MaxImprovisationCount; improvisationCount++)
            {
                var worstHarmony = HarmonyMemory.WorstHarmony;

                // Get dynamic parameters
                var (considerationRatio, pitchAdjustmentRatio) = _parameterProvider.GetParameterSet();

                HarmonyGenerator.HarmonyMemoryConsiderationRatio = considerationRatio;
                HarmonyGenerator.PitchAdjustmentRatio = pitchAdjustmentRatio;

                // Improvise harmony with the new parameters
                var improvisedHarmony = HarmonyGenerator.ImproviseHarmony();

                if (improvisedHarmony.IsBetterThan(worstHarmony) && !HarmonyMemory.Contains(improvisedHarmony))
                {
                    HarmonyMemory.SwapWithWorstHarmony(improvisedHarmony);

                    // Mark parameter set as winning if if harmony is better
                    _parameterProvider.MarkCurrentParametersAsWinning();
                }
            }

            return HarmonyMemory.BestHarmony;
        }
    }
}