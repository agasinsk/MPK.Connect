using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    public interface IHarmonyGenerator<T>
    {
        HarmonyMemory<T> HarmonyMemory { get; set; }
        double HarmonyMemoryConsiderationRatio { get; set; }
        IObjectiveFunction<T> ObjectiveFunction { get; }
        double PitchAdjustmentRatio { get; set; }
        HarmonyGeneratorType Type { get; }

        Harmony<T> GenerateRandomHarmony();

        Harmony<T> ImproviseHarmony();
    }
}