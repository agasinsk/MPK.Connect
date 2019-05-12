using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Generator;
using MPK.Connect.Service.Business.HarmonySearch.ParameterProviders;

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

        int ImprovisationCount { get; }
        long MaxImprovisationCount { get; }
        ObjectiveFunctionType ObjectiveFunctionType { get; }
        IParameterProvider ParameterProvider { get; }
        HarmonySearchType Type { get; }

        void Reset();

        Harmony<T> SearchForHarmony();
    }
}