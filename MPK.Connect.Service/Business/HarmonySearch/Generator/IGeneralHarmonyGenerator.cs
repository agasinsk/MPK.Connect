using MPK.Connect.Service.Business.HarmonySearch.Core;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    public interface IGeneralHarmonyGenerator<T> : IHarmonyGenerator<T>
    {
        /// <summary>
        /// Gets a collection of random arguments
        /// </summary>
        /// <remarks>Note. This method does not rely on constant argument count (or their indexes)</remarks>
        /// <returns>Random arguments</returns>
        T[] GetRandomArguments();

        /// <summary>
        /// Uses pitch adjustment technique to generate new solution
        /// </summary>
        /// <param name="harmony">Harmony retrieved from harmony memory</param>
        /// <returns>Pitch adjusted harmony</returns>
        Harmony<T> UsePitchAdjustment(Harmony<T> harmony);
    }
}