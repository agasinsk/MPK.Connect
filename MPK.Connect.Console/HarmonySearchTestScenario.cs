using System;
using System.Collections.Generic;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Generator;

namespace MPK.Connect.Console
{
    public class HarmonySearchTestScenario<T> where T : class
    {
        private readonly List<Type> _functionTypes;
        private readonly List<Type> _harmonyGeneratorTypes;
        private readonly List<Type> _harmonySearcherTypes;
        public List<HarmonySearchTestSettings<T>> Settings { get; set; }

        public HarmonySearchTestScenario()
        {
            _functionTypes = new List<Type>
            {
                typeof(TravelTimeObjectiveFunction)
            };

            _harmonyGeneratorTypes = new List<Type>
            {
                typeof(RandomStopTimeHarmonyGenerator),
                typeof(RandomStopHarmonyGenerator),
                typeof(DirectedStopTimeHarmonyGenerator)
            };

            _harmonySearcherTypes = new List<Type>
            {
                typeof(HarmonySearcher<T>),
                typeof(ImprovedHarmonySearcher<T>),
                typeof(DynamicHarmonySearcher<T>),
                typeof(DividedHarmonySearcher<T>)
            };

            Settings = new List<HarmonySearchTestSettings<T>>();

            CreateDefaultSettings();
        }

        private void CreateDefaultSettings()
        {
            foreach (var harmonyGeneratorType in _harmonyGeneratorTypes)
            {
                foreach (var harmonySearcherType in _harmonySearcherTypes)
                {
                    foreach (var functionType in _functionTypes)
                    {
                        var testSettings = new HarmonySearchTestSettings<T>
                        {
                            HarmonyGeneratorType = harmonyGeneratorType,
                            HarmonySearcherType = harmonySearcherType,
                            ObjectiveFunctionType = functionType
                        };
                        Settings.Add(testSettings);
                    }
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