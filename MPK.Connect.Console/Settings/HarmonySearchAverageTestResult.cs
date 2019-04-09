using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Generator;

namespace MPK.Connect.TestEnvironment.Settings
{
    public class HarmonySearchAverageTestResult
    {
        public double AverageObjectiveFunctionValue { get; set; }

        public double AverageTime { get; set; }

        public double BestObjectiveFunctionValue { get; set; }

        public int FeasibleSolutionsCount { get; set; }

        public HarmonyGeneratorType HarmonyGeneratorType { get; set; }

        public HarmonySearchType HarmonySearchType { get; set; }
        public int ImprovisationCount { get; set; }

        public int NonFeasibleCount { get; set; }

        public ObjectiveFunctionType ObjectiveFunctionType { get; set; }

        public double WorstObjectiveFunctionValue { get; set; }

        public HarmonySearchAverageTestResult(HarmonySearchType harmonySearchType, HarmonyGeneratorType harmonyGeneratorType, ObjectiveFunctionType objectiveFunctionType, List<HarmonySearchTestResult> testResults)
        {
            HarmonySearchType = harmonySearchType;
            ObjectiveFunctionType = objectiveFunctionType;
            HarmonyGeneratorType = harmonyGeneratorType;

            BestObjectiveFunctionValue = testResults.Min(r => r.BestHarmony.ObjectiveValue);
            WorstObjectiveFunctionValue = testResults.Max(r => r.BestHarmony.ObjectiveValue);
            AverageObjectiveFunctionValue =
                testResults.All(r => double.IsPositiveInfinity(r.BestHarmony.ObjectiveValue))
                    ? double.PositiveInfinity
                    : testResults.Select(r => r.BestHarmony.ObjectiveValue)
                        .Where(d => !double.IsPositiveInfinity(d)).Average();
            AverageTime = testResults.Average(r => r.Time.TotalSeconds);
            NonFeasibleCount = testResults.Count(r => double.IsPositiveInfinity(r.BestHarmony.ObjectiveValue));
            ImprovisationCount = (int)testResults.Average(r => r.ImprovisationCount);
            FeasibleSolutionsCount = testResults.Count(r => !double.IsPositiveInfinity(r.BestHarmony.ObjectiveValue));
        }

        public HarmonySearchAverageTestResult()
        {
        }

        public HarmonySearchAverageTestResult(HarmonySearchType harmonySearchType, HarmonyGeneratorType harmonyGeneratorType, ObjectiveFunctionType objectiveFunctionType)
        {
            HarmonySearchType = harmonySearchType;
            HarmonyGeneratorType = harmonyGeneratorType;
            ObjectiveFunctionType = objectiveFunctionType;
        }
    }
}