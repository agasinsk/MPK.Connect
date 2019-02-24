using System;

namespace MPK.Connect.Service.Business.HarmonySearch
{
    public class GeneralArgumentHarmonyGenerator<T> : HarmonyGeneratorBase<T>
    {
        protected new IGeneralObjectiveFunction<T> Function;

        public GeneralArgumentHarmonyGenerator(IGeneralObjectiveFunction<T> function, HarmonyMemory<T> harmonyMemory, double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio) : base(function, harmonyMemory, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio)
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
            var generationRule = EstablishArgumentGenerationRule(randomValue);

            switch (generationRule)
            {
                case ArgumentGenerationRules.MemoryConsideration:
                    return UseMemoryConsideration();

                case ArgumentGenerationRules.PitchAdjustment:
                    return UsePitchAdjustment();

                case ArgumentGenerationRules.RandomChoosing:
                    return UseRandomChoosing();

                default:
                    return null;
            }
        }

        public Harmony<T> UseMemoryConsideration()
        {
            return HarmonyMemory.GetRandomHarmony();
        }

        private Harmony<T> UsePitchAdjustment()
        {
            throw new NotImplementedException();
        }

        private Harmony<T> UseRandomChoosing()
        {
            return GenerateRandomHarmony();
        }
    }
}