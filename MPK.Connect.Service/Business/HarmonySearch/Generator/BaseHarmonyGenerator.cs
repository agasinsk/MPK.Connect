using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;
using MPK.Connect.Service.Utils;
using System;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    public abstract class BaseHarmonyGenerator<T> : IHarmonyGenerator<T>
    {
        protected readonly IObjectiveFunction<T> ObjectiveFunction;
        protected readonly IBoundedRandom Random;
        public HarmonyMemory<T> HarmonyMemory { get; set; }

        public ObjectiveFunctionType ObjectiveFunctionType => ObjectiveFunction.Type;
        public abstract HarmonyGeneratorType Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseHarmonyGenerator{T}"/> class.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <exception cref="ArgumentNullException">function</exception>
        protected BaseHarmonyGenerator(IObjectiveFunction<T> function)
        {
            ObjectiveFunction = function ?? throw new ArgumentNullException(nameof(function));

            Random = RandomFactory.GetInstance();
        }

        /// <summary>
        /// Establishes the harmony generation rule.
        /// </summary>
        /// <param name="probability">The probability.</param>
        /// <param name="harmonyMemoryConsiderationRatio">The harmony memory consideration ratio.</param>
        /// <param name="pitchAdjustmentRatio">The pitch adjustment ratio.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Generates the random harmony.
        /// </summary>
        /// <returns></returns>
        public abstract Harmony<T> GenerateRandomHarmony();

        /// <summary>
        /// Improvises the harmony.
        /// </summary>
        /// <param name="harmonyMemoryConsiderationRatio">The harmony memory consideration ratio.</param>
        /// <param name="pitchAdjustmentRatio">The pitch adjustment ratio.</param>
        /// <returns></returns>
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
                    return new Harmony<T>(double.PositiveInfinity);
            }
        }

        /// <summary>
        /// Pitches the adjust harmony.
        /// </summary>
        /// <param name="harmony">The harmony.</param>
        /// <returns></returns>
        public abstract Harmony<T> PitchAdjustHarmony(Harmony<T> harmony);

        /// <summary>
        /// Uses the memory consideration.
        /// </summary>
        /// <returns></returns>
        public virtual Harmony<T> UseMemoryConsideration()
        {
            return HarmonyMemory.GetRandomElement();
        }

        /// <summary>
        /// Uses the pitch adjustment.
        /// </summary>
        /// <returns></returns>
        public Harmony<T> UsePitchAdjustment()
        {
            var harmonyFromMemory = UseMemoryConsideration();

            var pitchAdjustedHarmony = PitchAdjustHarmony(harmonyFromMemory);

            return pitchAdjustedHarmony;
        }

        /// <summary>
        /// Uses the random choosing.
        /// </summary>
        /// <returns></returns>
        public Harmony<T> UseRandomChoosing()
        {
            return GenerateRandomHarmony();
        }
    }
}