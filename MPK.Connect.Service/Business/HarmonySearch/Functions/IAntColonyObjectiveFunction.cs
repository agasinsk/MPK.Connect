using System.Collections.Generic;
using MPK.Connect.Service.Business.HarmonySearch.Core;

namespace MPK.Connect.Service.Business.HarmonySearch.Functions
{
    public interface IAntColonyObjectiveFunction<T> : IGeneralObjectiveFunction<T>
    {
        IEnumerable<Harmony<T>> GetAntSolutions();
    }
}