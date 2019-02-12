using System;

namespace MPK.Connect.Service.Business.HarmonySearch
{
    public class ContinuousHarmonyGenerator<T> : HarmonyGeneratorBase<T>
    {
        protected new IContinuousObjectiveFunction<T> Function;

        public ContinuousHarmonyGenerator(IContinuousObjectiveFunction<T> function, HarmonyMemory<T> harmonyMemory, double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio) : base(function, harmonyMemory, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));
        }

        public override T UsePitchAdjustment(int argumentIndex)
        {
            var existingValue = UseMemoryConsideration(argumentIndex);
            var randomValue = Random.NextDouble();

            if (randomValue < 0.5)
            {
                return Function.GetPitchDownAdjustedValue(argumentIndex, existingValue);
            }

            return Function.GetPitchUpAdjustedValue(argumentIndex, existingValue);
        }
    }
}