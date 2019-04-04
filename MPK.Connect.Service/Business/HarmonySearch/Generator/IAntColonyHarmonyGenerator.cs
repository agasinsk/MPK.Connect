using System.Collections.Generic;
using MPK.Connect.Service.Business.HarmonySearch.Core;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    public interface IAntColonyHarmonyGenerator<T> : IHarmonyGenerator<T>
    {
        IEnumerable<Harmony<T>> GetAntSolutions();
    }
}