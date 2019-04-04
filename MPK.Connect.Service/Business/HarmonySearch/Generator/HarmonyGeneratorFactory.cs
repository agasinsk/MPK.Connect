namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    public class HarmonyGeneratorFactory
    {
        //public static IHarmonyGenerator<T> GetHarmonyGenerator<T>(IObjectiveFunction<T> function,
        //    HarmonyMemory<T> harmonyMemory, double harmonyMemoryConsiderationRatio = double.NegativeInfinity, double pitchAdjustmentRatio = double.NegativeInfinity)
        //{
        //    switch (function)
        //    {
        //        case IContinuousObjectiveFunction<T> continuousObjectiveFunction:
        //            return new ContinuousArgumentHarmonyGenerator<T>(continuousObjectiveFunction, harmonyMemory, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio);

        // case IDiscreteObjectiveFunction<T> discreteObjectiveFunction: return new
        // DiscreteArgumentHarmonyGenerator<T>(discreteObjectiveFunction, harmonyMemory,
        // harmonyMemoryConsiderationRatio, pitchAdjustmentRatio);

        // case IGeneralObjectiveFunction<T> generalObjectiveFunction: { if
        // (double.IsNegativeInfinity(harmonyMemoryConsiderationRatio) ||
        // double.IsNegativeInfinity(pitchAdjustmentRatio)) { return new
        // DynamicHarmonyGenerator<T>(generalObjectiveFunction, harmonyMemory); }

        //                return new GeneralHarmonyGenerator<T>(generalObjectiveFunction, harmonyMemory,
        //                    harmonyMemoryConsiderationRatio, pitchAdjustmentRatio);
        //            }
        //        default:
        //            return null;
        //    }
        //}
    }
}