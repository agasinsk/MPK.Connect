using System;

namespace MPK.Connect.Service.Business.HarmonySearch.Core
{
    public interface IHarmonySearcher<T>
    {
        HarmonyMemory<T> HarmonyMemory { get; }
        double HarmonyMemoryConsiderationRatio { get; }
        long MaxImprovisationCount { get; }
        double PitchAdjustmentRatio { get; set; }
        HarmonySearchType Type { get; }

        Type GetObjectiveFunctionType();

        /// <summary>
        /// Looks for optimal solution of a function
        /// </summary>
        Harmony<T> SearchForHarmony();
    }

    public interface IImprovedHarmonySearcher<T> : IHarmonySearcher<T>
    {
        double MaxPitchAdjustmentRatio { get; set; }
        double MinPitchAdjustmentRatio { get; set; }
    }
}