using System;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    public abstract class HarmonyGeneratorBase<T> : IHarmonyGenerator<T>
    {
        protected readonly IRandom Random;
        protected IObjectiveFunction<T> Function;
        public HarmonyMemory<T> HarmonyMemory { get; set; }
        public double HarmonyMemoryConsiderationRatio { get; set; }
        public double PitchAdjustmentRatio { get; set; }

        protected HarmonyGeneratorBase(IObjectiveFunction<T> function,
            HarmonyMemory<T> harmonyMemory,
            double harmonyMemoryConsiderationRatio,
            double pitchAdjustmentRatio)
        {
            HarmonyMemoryConsiderationRatio = harmonyMemoryConsiderationRatio;
            PitchAdjustmentRatio = pitchAdjustmentRatio;

            Random = RandomFactory.GetInstance();
            Function = function ?? throw new ArgumentNullException(nameof(function));
            HarmonyMemory = harmonyMemory ?? throw new ArgumentNullException(nameof(harmonyMemory));
        }

        public HarmonyGenerationRules EstablishHarmonyGenerationRule(double probability)
        {
            if (probability > HarmonyMemoryConsiderationRatio)
            {
                return HarmonyGenerationRules.RandomChoosing;
            }
            if (probability <= HarmonyMemoryConsiderationRatio * PitchAdjustmentRatio)
            {
                return HarmonyGenerationRules.PitchAdjustment;
            }

            return HarmonyGenerationRules.MemoryConsideration;
        }

        public abstract Harmony<T> GenerateRandomHarmony();

        public Harmony<T> GetHarmony(params T[] arguments)
        {
            var functionValue = Function.CalculateObjectiveValue(arguments);

            return new Harmony<T>(functionValue, arguments);
        }

        public abstract Harmony<T> ImproviseHarmony();
    }
}