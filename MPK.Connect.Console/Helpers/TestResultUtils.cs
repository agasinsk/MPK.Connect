using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Generator;
using MPK.Connect.TestEnvironment.Settings;

namespace MPK.Connect.TestEnvironment.Helpers
{
    internal class TestResultUtils
    {
        /// <summary>
        /// Groups the test results.
        /// </summary>
        /// <param name="testResults">The test results.</param>
        /// <returns></returns>
        internal static Dictionary<string, Dictionary<string, List<HarmonySearchAverageTestResult>>> GroupTestResults(List<HarmonySearchAverageTestResult> testResults)
        {
            return new Dictionary<string, Dictionary<string, List<HarmonySearchAverageTestResult>>>
            {
                [nameof(HarmonySearchType)] = testResults
                    .GroupBy(r => r.HarmonySearchType)
                    .ToDictionary(k => k.Key.ToString(), v => v.ToList()),

                [nameof(HarmonyGeneratorType)] = testResults
                    .GroupBy(r => r.HarmonyGeneratorType)
                    .ToDictionary(k => k.Key.ToString(), v => v.ToList()),

                [nameof(ObjectiveFunctionType)] = testResults
                    .GroupBy(r => r.ObjectiveFunctionType)
                    .ToDictionary(k => k.Key.ToString(), v => v.ToList()),
            };
        }
    }
}