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
        /// <summary>
        /// Gets the ant colony solutions.
        /// </summary>
        /// <param name="solutionCount">The solution count.</param>
        /// <returns></returns>
        IEnumerable<Harmony<T>> GetAntColonySolutions(int solutionCount);

        /// <summary>
        /// Resets this instance.
        /// </summary>
        void Reset();

        /// <summary>
        /// Updates the global pheromone.
        /// </summary>
        /// <param name="bestHarmony">The best harmony.</param>
        void UpdateGlobalPheromone(Harmony<T> bestHarmony);
    }
}