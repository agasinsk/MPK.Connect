using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Generator;

namespace MPK.Connect.TestEnvironment.Settings
{
    public class HarmonySearchAverageTestResult : AverageTestResult
    {
        public HarmonyGeneratorType HarmonyGeneratorType { get; set; }

        public HarmonySearchType HarmonySearchType { get; set; }
        public int ImprovisationCount { get; set; }

        public ObjectiveFunctionType ObjectiveFunctionType { get; set; }

        public HarmonySearchAverageTestResult(HarmonySearchType harmonySearchType, HarmonyGeneratorType harmonyGeneratorType, ObjectiveFunctionType objectiveFunctionType, IEnumerable<HarmonySearchTestResult> testResults) : base(testResults)
        {
            HarmonySearchType = harmonySearchType;
            ObjectiveFunctionType = objectiveFunctionType;
            HarmonyGeneratorType = harmonyGeneratorType;

            ImprovisationCount = (int)testResults.Average(r => r.ImprovisationCount);
        }
    }
}