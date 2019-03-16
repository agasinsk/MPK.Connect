using System;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    /// <summary>
    /// Generates harmonies
    /// </summary>
    public class DiscreteArgumentHarmonyGenerator<T> : ArgumentHarmonyGenerator<T>
    {
        protected new IDiscreteObjectiveFunction<T> ObjectiveFunction;

        public DiscreteArgumentHarmonyGenerator(IDiscreteObjectiveFunction<T> function, HarmonyMemory<T> harmonyMemory, double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio) : base(function, harmonyMemory, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio)
        {
            ObjectiveFunction = function ?? throw new ArgumentNullException(nameof(function));
        }

        public override T UseMemoryConsideration(int argumentIndex)
        {
            var argumentFromRandomHarmony = HarmonyMemory.GetArgumentFromRandomHarmony(argumentIndex);
            while (!ObjectiveFunction.IsArgumentValuePossible(argumentFromRandomHarmony))
            {
                argumentFromRandomHarmony = ObjectiveFunction.GetArgumentValue(argumentIndex);
            }
            ObjectiveFunction.SaveArgumentValue(argumentIndex, argumentFromRandomHarmony);
            return argumentFromRandomHarmony;
        }

        public override T UsePitchAdjustment(int argumentIndex)
        {
            var valueFromHarmonyMemory = HarmonyMemory.GetArgumentFromRandomHarmony(argumentIndex);

            var pitchAdjustedValue = ObjectiveFunction.GetNeighborValue(argumentIndex, valueFromHarmonyMemory);
            ObjectiveFunction.SaveArgumentValue(argumentIndex, pitchAdjustedValue);
            return pitchAdjustedValue;
        }

        public override T UseRandomChoosing(int argumentIndex)
        {
            var randomArgumentValue = ObjectiveFunction.GetArgumentValue(argumentIndex);
            ObjectiveFunction.SaveArgumentValue(argumentIndex, randomArgumentValue);
            return randomArgumentValue;
        }
    }
}