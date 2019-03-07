using System.Collections.Generic;
using MPK.Connect.Service.Business.HarmonySearch.Core;

namespace MPK.Connect.Service.Business.HarmonySearch.Helpers
{
    public class HarmonyComparer<T> : IComparer<Harmony<T>>
    {
        public int Compare(Harmony<T> firstHarmony, Harmony<T> secondHarmony)
        {
            var objectiveValueComparison = firstHarmony.ObjectiveValue.CompareTo(secondHarmony.ObjectiveValue);

            if (objectiveValueComparison == 0)
            {
                return firstHarmony.Arguments.Length.CompareTo(secondHarmony.Arguments.Length);
            }

            return objectiveValueComparison;
        }
    }
}