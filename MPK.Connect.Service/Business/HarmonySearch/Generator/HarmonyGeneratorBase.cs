using System;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    public abstract class HarmonyGeneratorBase<T> : IHarmonyGenerator<T>
    {
        protected readonly IBoundedRandom Random;
        public HarmonyMemory<T> HarmonyMemory { get; set; }
        public double HarmonyMemoryConsiderationRatio { get; set; }
        public IObjectiveFunction<T> ObjectiveFunction { get; }
        public double PitchAdjustmentRatio { get; set; }

        protected HarmonyGeneratorBase(IObjectiveFunction<T> function,
            HarmonyMemory<T> harmonyMemory)
        {
            Random = RandomFactory.GetInstance();
            ObjectiveFunction = function ?? throw new ArgumentNullException(nameof(function));
            HarmonyMemory = harmonyMemory ?? throw new ArgumentNullException(nameof(harmonyMemory));
        }

        protected HarmonyGeneratorBase(IObjectiveFunction<T> function,
            HarmonyMemory<T> harmonyMemory,
            double harmonyMemoryConsiderationRatio,
            double pitchAdjustmentRatio)
        {
            HarmonyMemoryConsiderationRatio = harmonyMemoryConsiderationRatio;
            PitchAdjustmentRatio = pitchAdjustmentRatio;

            Random = RandomFactory.GetInstance();
            ObjectiveFunction = function ?? throw new ArgumentNullException(nameof(function));
            HarmonyMemory = harmonyMemory ?? throw new ArgumentNullException(nameof(harmonyMemory));
        }

        public virtual HarmonyGenerationRules EstablishHarmonyGenerationRule(double probability)
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
            var functionValue = ObjectiveFunction.CalculateObjectiveValue(arguments);

            return new Harmony<T>(functionValue, arguments);
        }

        public abstract Harmony<T> ImproviseHarmony();

        public virtual void MarkCurrentParametersAsWinning()
        {
        }
    }
}