using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Generator;

namespace MPK.Connect.TestEnvironment.Settings
{
    public class HarmonySearchTestResult : TestResult
    {
        public HarmonyGeneratorType HarmonyGeneratorType { get; set; }
        public HarmonySearchType HarmonySearchType { get; set; }
        public int ImprovisationCount { get; set; }
        public ObjectiveFunctionType ObjectiveFunctionType { get; set; }
    }
}