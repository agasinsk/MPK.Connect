using System;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    public class GeneralHarmonyGenerator<T> : HarmonyGeneratorBase<T>
    {
        protected new IGeneralObjectiveFunction<T> ObjectiveFunction;

        public GeneralHarmonyGenerator(IObjectiveFunction<T> function,
            HarmonyMemory<T> harmonyMemory) : base(function, harmonyMemory)
        {
        }

        public GeneralHarmonyGenerator(IGeneralObjectiveFunction<T> function, HarmonyMemory<T> harmonyMemory, double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio) : base(function, harmonyMemory, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio)
        {
            ObjectiveFunction = function ?? throw new ArgumentNullException(nameof(function));
        }

        public override Harmony<T> GenerateRandomHarmony()
        {
            var arguments = ObjectiveFunction.GetRandomArguments();

            return GetHarmony(arguments);
        }

        public override Harmony<T> ImproviseHarmony()
        {
            var randomValue = Random.NextDouble();
            var generationRule = EstablishHarmonyGenerationRule(randomValue);

            switch (generationRule)
            {
                case HarmonyGenerationRules.MemoryConsideration:
                    return UseMemoryConsideration();

                case HarmonyGenerationRules.PitchAdjustment:
                    return UsePitchAdjustment();

                case HarmonyGenerationRules.RandomChoosing:
                    return UseRandomChoosing();

                default:
                    return new Harmony<T>(double.MaxValue);
            }
        }

        public override void MarkCurrentParametersAsWinning()
        {
        }

        public Harmony<T> UseMemoryConsideration()
        {
            return HarmonyMemory.GetRandomElement();
        }

        private Harmony<T> UsePitchAdjustment()
        {
            var harmonyFromMemory = UseMemoryConsideration();

            var pitchAdjustedHarmony = ObjectiveFunction.UsePitchAdjustment(harmonyFromMemory);

            return pitchAdjustedHarmony;
        }

        private Harmony<T> UseRandomChoosing()
        {
            return GenerateRandomHarmony();
        }
    }
}