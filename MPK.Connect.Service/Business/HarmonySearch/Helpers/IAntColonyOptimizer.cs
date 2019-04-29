using MPK.Connect.Service.Business.HarmonySearch.Core;
using System.Collections.Generic;

namespace MPK.Connect.Service.Business.HarmonySearch.Helpers
{
    /// <summary>
    /// The interface for Ant Colony optimizer
    /// </summary>
    /// <typeparam name="T">Type of solution elements</typeparam>
    public interface IAntColonyOptimizer<T>
    {
        IEnumerable<Harmony<T>> GetAntColonySolutions(int solutionCount);

        void Reset();

        void UpdateGlobalPheromone(Harmony<T> bestHarmony);
    }
}