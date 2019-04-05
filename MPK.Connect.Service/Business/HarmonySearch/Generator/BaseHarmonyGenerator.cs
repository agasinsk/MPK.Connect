using System;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    public abstract class BaseHarmonyGenerator<T> : IHarmonyGenerator<T>
    {
        protected readonly IObjectiveFunction<T> ObjectiveFunction;
        protected readonly IBoundedRandom Random;
        public HarmonyMemory<T> HarmonyMemory { get; set; }

        public abstract HarmonyGeneratorType Type { get; }

        protected BaseHarmonyGenerator(IObjectiveFunction<T> function)
        {
            ObjectiveFunction = function ?? throw new ArgumentNullException(nameof(function));

            Random = RandomFactory.GetInstance();
        }

        public virtual HarmonyGenerationRules EstablishHarmonyGenerationRule(double probability, double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio)
        {
            if (probability > harmonyMemoryConsiderationRatio)
            {
                return HarmonyGenerationRules.RandomChoosing;
            }
            if (probability <= harmonyMemoryConsiderationRatio * pitchAdjustmentRatio)
            {
                return HarmonyGenerationRules.PitchAdjustment;
            }

            return HarmonyGenerationRules.MemoryConsideration;
        }

        public abstract Harmony<T> GenerateRandomHarmony();

        public Harmony<T> ImproviseHarmony(double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio)
        {
            var randomValue = Random.NextDouble();
            var generationRule = EstablishHarmonyGenerationRule(randomValue, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio);

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

        public abstract Harmony<T> PitchAdjustHarmony(Harmony<T> harmony);

        public virtual Harmony<T> UseMemoryConsideration()
        {
            return HarmonyMemory.GetRandomElement();
        }

        public Harmony<T> UsePitchAdjustment()
        {
            var harmonyFromMemory = UseMemoryConsideration();

            var pitchAdjustedHarmony = PitchAdjustHarmony(harmonyFromMemory);

            return pitchAdjustedHarmony;
        }

        public Harmony<T> UseRandomChoosing()
        {
            return GenerateRandomHarmony();
        }
    }
}