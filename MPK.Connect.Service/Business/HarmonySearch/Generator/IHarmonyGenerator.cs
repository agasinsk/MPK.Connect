using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    /// <summary>
    /// The interface for harmony generator
    /// </summary>
    /// <typeparam name="T">Type of harmony elements</typeparam>
    public interface IHarmonyGenerator<T>
    {
        HarmonyMemory<T> HarmonyMemory { get; set; }

        HarmonyGeneratorType Type { get; }

        Harmony<T> GenerateRandomHarmony();

        Harmony<T> ImproviseHarmony(double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio);
    }
}