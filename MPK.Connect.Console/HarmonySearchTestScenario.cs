using System;
using System.Collections.Generic;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;

namespace MPK.Connect.Console
{
    public class HarmonySearchTestScenario<T> where T : class
    {
        public List<HarmonySearchTestSettings<T>> Settings { get; set; }

        public HarmonySearchTestScenario(Type harmonySearcherType = null, Type objectiveFunctionType = null)
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

            if (harmonySearcherType != null && objectiveFunctionType == null)
            {
                CreateSettingsForSingleHarmonySearcherType(harmonySearcherType, functionTypes);
            }
            if (objectiveFunctionType != null && harmonySearcherType == null)
            {
                CreateSettingsForSingleObjectiveFunctionType(objectiveFunctionType, harmonySearcherTypes);
            }
            if (harmonySearcherType != null && objectiveFunctionType != null)
            {
                CreateSingleSetting(harmonySearcherType, objectiveFunctionType);
            }
            if (harmonySearcherType == null && objectiveFunctionType == null)
            {
                CreateDefaultSettings(functionTypes, harmonySearcherTypes);
            }
        }

        private void CreateDefaultSettings(List<Type> functionTypes, List<Type> harmonySearcherTypes)
        {
            foreach (var functionType in functionTypes)
            {
                foreach (var harmonySearcherType in harmonySearcherTypes)
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

        private void CreateSettingsForSingleHarmonySearcherType(Type harmonySearcherType, List<Type> functionTypes)
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

        private void CreateSettingsForSingleObjectiveFunctionType(Type functionType, List<Type> harmonySearcherTypes)
        {
            foreach (var harmonySearcherType in harmonySearcherTypes)
            {
                var testSettings = new HarmonySearchTestSettings<T>
                {
                    HarmonySearcherType = harmonySearcherType,
                    ObjectiveFunctionType = functionType
                };
                Settings.Add(testSettings);
            }
        }

        private void CreateSingleSetting(Type harmonySearcherType, Type objectiveFunctionType)
        {
            var testSettings = new HarmonySearchTestSettings<T>
            {
                HarmonySearcherType = harmonySearcherType,
                ObjectiveFunctionType = objectiveFunctionType
            };
            Settings.Add(testSettings);
        }
    }
}