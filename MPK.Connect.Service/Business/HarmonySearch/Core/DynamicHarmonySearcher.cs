using System;
using MPK.Connect.Service.Business.HarmonySearch.Constants;
using MPK.Connect.Service.Business.HarmonySearch.Generator;
using MPK.Connect.Service.Business.HarmonySearch.ParameterProviders;

namespace MPK.Connect.Service.Business.HarmonySearch.Core
{
    /// <summary>
    /// Implements harmony search with dynamically adjusted parameter values (ParameterSetList and WinningParameterSetList)
    /// </summary>
    /// <typeparam name="T">Type of entities</typeparam>
    public class DynamicHarmonySearcher<T> : HarmonySearcher<T>
    {
        public new IDynamicParameterProvider ParameterProvider { get; }

        public DynamicHarmonySearcher(IHarmonyGenerator<T> harmonyGenerator, IDynamicParameterProvider parameterProvider, int harmonyMemorySize = HarmonySearchConstants.DefaultHarmonyMemorySize, long maxImprovisationCount = HarmonySearchConstants.DefaultMaxImprovisationCount) : base(harmonyGenerator, parameterProvider, harmonyMemorySize, maxImprovisationCount)
        {
            ParameterProvider = parameterProvider ?? throw new ArgumentNullException(nameof(parameterProvider));
        }

        public override Harmony<T> SearchForHarmony()
        {
            InitializeHarmonyMemory();

            for (var improvisationCount = 0; improvisationCount < MaxImprovisationCount; improvisationCount++)
            {
                var worstHarmony = HarmonyMemory.WorstHarmony;

                // Get dynamic parameters
                var harmonyMemoryConsiderationRatio = ParameterProvider.HarmonyMemoryConsiderationRatio;
                var pitchAdjustmentRatio = ParameterProvider.PitchAdjustmentRatio;

                // Improvise harmony with the new parameters
                var improvisedHarmony = HarmonyGenerator.ImproviseHarmony(harmonyMemoryConsiderationRatio, pitchAdjustmentRatio);

                if (improvisedHarmony.IsBetterThan(worstHarmony) && !HarmonyMemory.Contains(improvisedHarmony))
                {
                    HarmonyMemory.SwapWithWorstHarmony(improvisedHarmony);

                    // Mark parameter set as winning if the harmony was better
                    ParameterProvider.MarkCurrentParametersAsWinning();
                }
            }

            return HarmonyMemory.BestHarmony;
        }
    }
}