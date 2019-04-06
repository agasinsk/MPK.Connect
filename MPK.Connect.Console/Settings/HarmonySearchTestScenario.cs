using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Generator;
using MPK.Connect.TestEnvironment.Helpers;

namespace MPK.Connect.TestEnvironment.Settings
{
    /// <summary>
    /// Generates test scenarios for HS tests
    /// </summary>
    public class HarmonySearchTestScenario
    {
        public List<HarmonySearchTestSettings> Settings { get; set; }

        public HarmonySearchTestScenario()
        {
            Settings = CreateSettings();
        }

        public HarmonySearchTestScenario(HarmonyGeneratorType generatorType)
        {
            var harmonyGeneratorTypes = new List<HarmonyGeneratorType> { generatorType };

            Settings = CreateSettings(harmonyGeneratorTypes, null, null);
        }

        public HarmonySearchTestScenario(ObjectiveFunctionType functionType)
        {
            var objectiveFunctionTypes = new List<ObjectiveFunctionType> { functionType };

            Settings = CreateSettings(null, null, objectiveFunctionTypes);
        }

        public HarmonySearchTestScenario(HarmonySearchType harmonySearchType)
        {
            var harmonySearchTypes = new List<HarmonySearchType> { harmonySearchType };

            Settings = CreateSettings(null, harmonySearchTypes);
        }

        public HarmonySearchTestScenario(ObjectiveFunctionType functionType, HarmonyGeneratorType generatorType)
        {
            var harmonyGeneratorTypes = new List<HarmonyGeneratorType> { generatorType };
            var objectiveFunctionTypes = new List<ObjectiveFunctionType> { functionType };

            Settings = CreateSettings(harmonyGeneratorTypes, null, objectiveFunctionTypes);
        }

        public HarmonySearchTestScenario(HarmonySearchType harmonySearchType, ObjectiveFunctionType functionType)
        {
            var harmonySearchTypes = new List<HarmonySearchType> { harmonySearchType };
            var objectiveFunctionTypes = new List<ObjectiveFunctionType> { functionType };

            Settings = CreateSettings(null, harmonySearchTypes, objectiveFunctionTypes);
        }

        public HarmonySearchTestScenario(HarmonySearchType harmonySearchType, HarmonyGeneratorType generatorType)
        {
            var harmonySearchTypes = new List<HarmonySearchType> { harmonySearchType };
            var harmonyGeneratorTypes = new List<HarmonyGeneratorType> { generatorType };

            Settings = CreateSettings(harmonyGeneratorTypes, harmonySearchTypes);
        }

        public HarmonySearchTestScenario(HarmonySearchType harmonySearchType, HarmonyGeneratorType generatorType, ObjectiveFunctionType functionType)
        {
            var objectiveFunctionTypes = new List<ObjectiveFunctionType> { functionType };
            var harmonySearchTypes = new List<HarmonySearchType> { harmonySearchType };
            var harmonyGeneratorTypes = new List<HarmonyGeneratorType> { generatorType };

            Settings = CreateSettings(harmonyGeneratorTypes, harmonySearchTypes, objectiveFunctionTypes);
        }

        private List<HarmonySearchTestSettings> CreateSettings(IEnumerable<HarmonyGeneratorType> harmonyGeneratorTypes = null, IEnumerable<HarmonySearchType> harmonySearchTypes = null, IEnumerable<ObjectiveFunctionType> objectiveFunctionTypes = null)
        {
            if (harmonyGeneratorTypes == null)
            {
                harmonyGeneratorTypes = EnumUtils<HarmonyGeneratorType>.GetEnumValues();
            }

            if (harmonySearchTypes == null)
            {
                harmonySearchTypes = EnumUtils<HarmonySearchType>.GetEnumValues();
            }

            if (objectiveFunctionTypes == null)
            {
                objectiveFunctionTypes = EnumUtils<ObjectiveFunctionType>.GetEnumValues();
            }

            var harmonySearchTestSettings = from harmonyGeneratorType in harmonyGeneratorTypes
                                            from harmonySearcherType in harmonySearchTypes
                                            from functionType in objectiveFunctionTypes
                                            select new HarmonySearchTestSettings
                                            {
                                                HarmonyGeneratorType = harmonyGeneratorType,
                                                HarmonySearcherType = harmonySearcherType,
                                                ObjectiveFunctionType = functionType
                                            };

            return harmonySearchTestSettings.ToList();
        }
    }
}