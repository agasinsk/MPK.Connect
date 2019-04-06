using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Generator;

namespace MPK.Connect.Service.Business.HarmonySearch.Core
{
    /// <summary>
    /// The interface for harmony searchers
    /// </summary>
    /// <typeparam name="T">Type of solution elements</typeparam>
    public interface IHarmonySearcher<T>
    {
        HarmonyGeneratorType HarmonyGeneratorType { get; }
        HarmonyMemory<T> HarmonyMemory { get; }

        long MaxImprovisationCount { get; }
        ObjectiveFunctionType ObjectiveFunctionType { get; }
        HarmonySearchType Type { get; }

        /// <summary>
        /// Looks for optimal solution of a problem
        /// </summary>
        Harmony<T> SearchForHarmony();
    }
}