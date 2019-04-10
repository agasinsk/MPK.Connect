using System.Collections.Generic;
using System.Linq;

namespace MPK.Connect.TestEnvironment.Settings
{
    public class AverageTestResult
    {
        public double AverageObjectiveFunctionValue { get; set; }

        public double AverageTime { get; set; }

        public double BestObjectiveFunctionValue { get; set; }

        public int FeasibleSolutionsCount { get; set; }

        public int NonFeasibleCount { get; set; }

        public double WorstObjectiveFunctionValue { get; set; }

        public AverageTestResult(IEnumerable<TestResult> testResults)
        {
            BestObjectiveFunctionValue = testResults.Min(r => r.Solution.ObjectiveValue);
            WorstObjectiveFunctionValue = testResults.Max(r => r.Solution.ObjectiveValue);
            AverageObjectiveFunctionValue =
                testResults.All(r => double.IsPositiveInfinity(r.Solution.ObjectiveValue))
                    ? double.PositiveInfinity
                    : testResults.Select(r => r.Solution.ObjectiveValue)
                        .Where(d => !double.IsPositiveInfinity(d)).Average();
            AverageTime = testResults.Average(r => r.Time.TotalSeconds);
            NonFeasibleCount = testResults.Count(r => double.IsPositiveInfinity(r.Solution.ObjectiveValue));
            FeasibleSolutionsCount = testResults.Count(r => !double.IsPositiveInfinity(r.Solution.ObjectiveValue));
        }

        public AverageTestResult()
        {
        }
    }
}