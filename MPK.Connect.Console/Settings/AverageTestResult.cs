using System;
using System.Collections.Generic;
using System.Linq;

namespace MPK.Connect.TestEnvironment.Settings
{
    public class AverageTestResult
    {
        public double AverageObjectiveFunctionValue { get; }
        public double AverageTime { get; }
        public double BestObjectiveFunctionValue { get; }
        public int FeasibleSolutionsCount { get; }
        public int NonFeasibleCount { get; set; }
        public double ObjectiveFunctionValueStandardDeviation { get; }
        public double ObjectiveFunctionValueStandardError { get; }
        public double SuccessRatio => FeasibleSolutionsCount * 100d / (FeasibleSolutionsCount + NonFeasibleCount);
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

            ObjectiveFunctionValueStandardError = GetSE(testResults.Where(r => !double.IsPositiveInfinity(r.Solution.ObjectiveValue)).ToList());
            ObjectiveFunctionValueStandardDeviation =
                GetStandardDeviation(testResults.Where(r => !double.IsPositiveInfinity(r.Solution.ObjectiveValue)).ToList());
        }

        public AverageTestResult()
        {
        }

        private double GetSE(List<TestResult> testResults)
        {
            return Math.Sqrt(testResults.Sum(r => Math.Pow(r.Solution.ObjectiveValue - AverageObjectiveFunctionValue, 2)) / testResults.Count) / Math.Sqrt(testResults.Count);
        }

        private double GetStandardDeviation(List<TestResult> testResults)
        {
            return Math.Sqrt(testResults.Sum(r => Math.Pow(r.Solution.ObjectiveValue - AverageObjectiveFunctionValue, 2)) / testResults.Count);
        }
    }
}