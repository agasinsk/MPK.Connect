using System;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;
using static MPK.Connect.Service.Business.HarmonySearch.Constants.HarmonySearchConstants;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    /// <summary>
    /// Dynamic harmony generator with dynamically adjusted parameter values (PSL and WPSL)
    /// </summary>
    /// <typeparam name="T">Type of arguments</typeparam>
    public class DynamicHarmonyGenerator<T> : GeneralHarmonyGenerator<T>
    {
        private readonly DynamicParameterProvider _parameterProvider;

        public DynamicHarmonyGenerator(IGeneralObjectiveFunction<T> function, HarmonyMemory<T> harmonyMemory) : base(function, harmonyMemory)
        {
            _parameterProvider = new DynamicParameterProvider(DefaultParameterListCapacity);
        }

        public override HarmonyGenerationRules EstablishHarmonyGenerationRule(double probability)
        {
            var (considerationRatio, pitchAdjustmentRatio) = _parameterProvider.GetParameterSet();

            HarmonyMemoryConsiderationRatio = considerationRatio;
            PitchAdjustmentRatio = pitchAdjustmentRatio;

            return base.EstablishHarmonyGenerationRule(probability);
        }

        public override void MarkCurrentParametersAsWinning()
        {
            _parameterProvider.MarkCurrentParametersAsWinning();
        }
    }
}