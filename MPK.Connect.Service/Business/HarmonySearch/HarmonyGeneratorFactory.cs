using MPK.Connect.Service.Business.HarmonySearch.Helpers;

namespace MPK.Connect.Service.Business.HarmonySearch
{
    public class HarmonyGeneratorFactory
    {
        public static IHarmonyGenerator<T> GetHarmonyGenerator<T>(IObjectiveFunction<T> function,
            HarmonyMemory<T> harmonyMemory, double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio,
            IRandom random = null)
        {
            if (function is IContinuousObjectiveFunction<T> continuousObjectiveFunction)
            {
                return new ContinuousHarmonyGenerator<T>(continuousObjectiveFunction, harmonyMemory, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio);
            }

            if (function is IDiscreteObjectiveFunction<T> discreteObjectiveFunction)
            {
                return new DiscreteHarmonyGenerator<T>(discreteObjectiveFunction, harmonyMemory, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio);
            }

            return null;
        }
    }
}