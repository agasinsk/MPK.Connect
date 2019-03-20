using MPK.Connect.Service.Business.HarmonySearch.Functions;

namespace MPK.Connect.Service.Business.HarmonySearch.Core
{
    public interface IHarmonySearcher<T>
    {
        HarmonyMemory<T> HarmonyMemory { get; }
        double HarmonyMemoryConsiderationRatio { get; }
        long MaxImprovisationCount { get; }
        ObjectiveFunctionType ObjectiveFunctionType { get; }
        double PitchAdjustmentRatio { get; set; }
        HarmonySearchType Type { get; }

        /// <summary>
        /// Looks for optimal solution of a problem
        /// </summary>
        Harmony<T> SearchForHarmony();
    }

    public interface IImprovedHarmonySearcher<T> : IHarmonySearcher<T>
    {
        double MaxPitchAdjustmentRatio { get; set; }
        double MinPitchAdjustmentRatio { get; set; }
    }
}