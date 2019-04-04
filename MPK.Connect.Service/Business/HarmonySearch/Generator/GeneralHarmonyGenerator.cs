using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    public abstract class GeneralHarmonyGenerator<T> : BaseHarmonyGenerator<T>, IGeneralHarmonyGenerator<T>
    {
        protected GeneralHarmonyGenerator(IObjectiveFunction<T> function) : base(function)
        {
        }

        public override Harmony<T> GenerateRandomHarmony()
        {
            var arguments = GetRandomArguments();

            return GetHarmony(arguments);
        }

        public abstract T[] GetRandomArguments();

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

        public abstract Harmony<T> UsePitchAdjustment(Harmony<T> harmony);

        public Harmony<T> UsePitchAdjustment()
        {
            var harmonyFromMemory = UseMemoryConsideration();

            var pitchAdjustedHarmony = UsePitchAdjustment(harmonyFromMemory);

            return pitchAdjustedHarmony;
        }

        public Harmony<T> UseRandomChoosing()
        {
            return GenerateRandomHarmony();
        }
    }
}