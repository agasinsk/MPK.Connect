using System.Collections.Generic;
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

        Harmony<T> GenerateRandomHarmony();

        //TODO: move to specific generator
        IEnumerable<Harmony<T>> GetAntSolutions();

        Harmony<T> ImproviseHarmony();

        //TODO: move to specific generator
        void MarkCurrentParametersAsWinning();
    }
}