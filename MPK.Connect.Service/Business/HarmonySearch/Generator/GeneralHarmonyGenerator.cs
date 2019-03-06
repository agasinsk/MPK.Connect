using System;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Helpers;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    public class GeneralHarmonyGenerator<T> : HarmonyGeneratorBase<T>
    {
        protected new IGeneralObjectiveFunction<T> Function;

        public GeneralHarmonyGenerator(IGeneralObjectiveFunction<T> function, HarmonyMemory<T> harmonyMemory, double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio) : base(function, harmonyMemory, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));
        }

        public override Harmony<T> GenerateRandomHarmony()
        {
            var arguments = Function.GetRandomArguments();

            return GetHarmony(arguments);
        }

        public override Harmony<T> ImproviseHarmony()
        {
            var randomValue = Random.NextDouble();
            var generationRule = EstablishHarmonyGenerationRule(randomValue);

            switch (generationRule)
            {
                case HarmonyGenerationRules.MemoryConsideration:
                    var fromMemory = UseMemoryConsideration();
                    return fromMemory;

                case HarmonyGenerationRules.PitchAdjustment:
                    return UsePitchAdjustment();

                case HarmonyGenerationRules.RandomChoosing:
                    return UseRandomChoosing();

                default:
                    return new Harmony<T>(double.MaxValue);
            }
        }

        public Harmony<T> UseMemoryConsideration()
        {
            return HarmonyMemory.GetRandomElement();
        }

        private Harmony<T> UsePitchAdjustment()
        {
            var harmonyFromMemory = UseMemoryConsideration();

            var pitchAdjustedHarmony = Function.UsePitchAdjustment(harmonyFromMemory);

            return pitchAdjustedHarmony;
        }

        private Harmony<T> UseRandomChoosing()
        {
            return GenerateRandomHarmony();
        }
    }
}