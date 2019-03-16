using System;
using System.Collections.Generic;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;

namespace MPK.Connect.Console
{
    public class HarmonySearchTestScenarios<T> where T : class
    {
        public List<HarmonySearchTestSettings<T>> Settings { get; set; }

        public HarmonySearchTestScenarios()
        {
            var functionTypes = new List<Type>
            {
                typeof(RandomStopTimeObjectiveFunction),
                typeof(RandomStopObjectiveFunction),
                typeof(DistanceStopTimeObjectiveFunction)
            };

            var harmonySearcherTypes = new List<Type>
            {
                typeof(HarmonySearcher<T>),
                typeof(ImprovedHarmonySearcher<T>),
                typeof(DividedHarmonySearcher<T>),
                typeof(DynamicHarmonySearcher<T>),
            };

            Settings = new List<HarmonySearchTestSettings<T>>();

            foreach (var harmonySearcherType in harmonySearcherTypes)
            {
                foreach (var functionType in functionTypes)
                {
                    var testSettings = new HarmonySearchTestSettings<T>
                    {
                        HarmonySearcherType = harmonySearcherType,
                        ObjectiveFunctionType = functionType
                    };
                    Settings.Add(testSettings);
                }
            }
        }
    }
}