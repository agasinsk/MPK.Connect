using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    /// <summary>
    /// Dynamic harmony generator with dynamically adjusted parameter values (PSL and WPSL)
    /// </summary>
    /// <typeparam name="T">Type of arguments</typeparam>
    public class DynamicHarmonyGenerator<T> : GeneralHarmonyGenerator<T>
    {
        private readonly DynamicParameterProvider _parameterProvider;

        public DynamicHarmonyGenerator(IGeneralObjectiveFunction<T> function, HarmonyMemory<T> harmonyMemory, double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio) : base(function, harmonyMemory, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio)
        {
            _parameterProvider = new DynamicParameterProvider(200);
        }

        public override HarmonyGenerationRules EstablishHarmonyGenerationRule(double probability)
        {
            var parameterSet = _parameterProvider.GetParameterSet();

            HarmonyMemoryConsiderationRatio = parameterSet.Item1;
            PitchAdjustmentRatio = parameterSet.Item2;

            return base.EstablishHarmonyGenerationRule(probability);
        }

        public override void MarkCurrentParametersAsWinning()
        {
            _parameterProvider.MarkCurrentParametersAsWinning();
        }
    }
}