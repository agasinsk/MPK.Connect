using MPK.Connect.Service.Business.HarmonySearch.Core;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    public interface IHarmonyGenerator<T>
    {
        HarmonyMemory<T> HarmonyMemory { get; set; }
        double HarmonyMemoryConsiderationRatio { get; set; }

        double PitchAdjustmentRatio { get; set; }

        Harmony<T> GenerateRandomHarmony();

        Harmony<T> ImproviseHarmony();

        void MarkCurrentParametersAsWinning();
    }
}