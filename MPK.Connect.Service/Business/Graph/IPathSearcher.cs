using System.Collections.Generic;
using MPK.Connect.Service.Business.HarmonySearch.Core;

namespace MPK.Connect.Service.Business.Graph
{
    public interface IPathSearcher<T>
    {
        IEnumerable<Harmony<T>> GetAvailablePaths();

        Harmony<T> GetShortestPath();
    }
}