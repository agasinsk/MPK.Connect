using System.Collections.Generic;
using MPK.Connect.Service.Business.HarmonySearch.Core;

namespace MPK.Connect.Service.Business.HarmonySearch.Helpers
{
    /// <summary>
    /// The interface for Ant Colony optimizer
    /// </summary>
    /// <typeparam name="T">Type of solution elements</typeparam>
    public interface IAntColonyOptimizer<T>
    {
        IEnumerable<Harmony<T>> GetAntSolutions(int solutionCount);

        void UpdateGlobalPheromone(Harmony<T> bestHarmony);
    }
}