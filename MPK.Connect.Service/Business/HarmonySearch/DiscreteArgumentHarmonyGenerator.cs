using System;

namespace MPK.Connect.Service.Business.HarmonySearch
{
    /// <summary>
    /// Generates harmonies
    /// </summary>
    public class DiscreteArgumentHarmonyGenerator<T> : ArgumentHarmonyGenerator<T>
    {
        protected new IDiscreteObjectiveFunction<T> Function;

        public DiscreteArgumentHarmonyGenerator(IDiscreteObjectiveFunction<T> function, HarmonyMemory<T> harmonyMemory, double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio) : base(function, harmonyMemory, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));
        }

        public override T UseMemoryConsideration(int argumentIndex)
        {
            var argumentFromRandomHarmony = HarmonyMemory.GetArgumentFromRandomHarmony(argumentIndex);
            while (!Function.IsArgumentValuePossible(argumentFromRandomHarmony))
            {
                argumentFromRandomHarmony = Function.GetArgumentValue(argumentIndex);
            }
            Function.SaveArgumentValue(argumentIndex, argumentFromRandomHarmony);
            return argumentFromRandomHarmony;
        }

        public override T UsePitchAdjustment(int argumentIndex)
        {
            var valueFromHarmonyMemory = HarmonyMemory.GetArgumentFromRandomHarmony(argumentIndex);

            var pitchAdjustedValue = Function.GetNeighborValue(argumentIndex, valueFromHarmonyMemory);
            Function.SaveArgumentValue(argumentIndex, pitchAdjustedValue);
            return pitchAdjustedValue;
        }

        public override T UseRandomChoosing(int argumentIndex)
        {
            var randomArgumentValue = Function.GetArgumentValue(argumentIndex);
            Function.SaveArgumentValue(argumentIndex, randomArgumentValue);
            return randomArgumentValue;
        }
    }
}