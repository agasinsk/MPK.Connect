using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    public class HarmonyGeneratorFactory
    {
        public static IHarmonyGenerator<T> GetHarmonyGenerator<T>(IObjectiveFunction<T> function,
            HarmonyMemory<T> harmonyMemory, double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio, bool shouldBeDynamic = false)
        {
            switch (function)
            {
                case IContinuousObjectiveFunction<T> continuousObjectiveFunction:
                    return new ContinuousArgumentHarmonyGenerator<T>(continuousObjectiveFunction, harmonyMemory, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio);

                case IDiscreteObjectiveFunction<T> discreteObjectiveFunction:
                    return new DiscreteArgumentHarmonyGenerator<T>(discreteObjectiveFunction, harmonyMemory, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio);

                case IGeneralObjectiveFunction<T> generalObjectiveFunction:
                    {
                        if (shouldBeDynamic)
                        {
                            return new DynamicHarmonyGenerator<T>(generalObjectiveFunction, harmonyMemory,
                                harmonyMemoryConsiderationRatio, pitchAdjustmentRatio);
                        }

                        return new GeneralHarmonyGenerator<T>(generalObjectiveFunction, harmonyMemory,
                        harmonyMemoryConsiderationRatio, pitchAdjustmentRatio);
                    }

                default:
                    return null;
            }
        }
    }
}