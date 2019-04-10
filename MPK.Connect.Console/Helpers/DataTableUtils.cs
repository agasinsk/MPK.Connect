using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.ParameterProviders;
using MPK.Connect.TestEnvironment.Settings;

namespace MPK.Connect.TestEnvironment.Helpers
{
    /// <summary>
    /// Data table utils
    /// </summary>
    internal class DataTableUtils
    {
        /// <summary>
        /// Gets the Harmony searcher parameter data table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="harmonySearcher">The harmony searcher.</param>
        /// <returns></returns>
        public static DataTable GetParameterDataTable<T>(IHarmonySearcher<T> harmonySearcher)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Property", typeof(string));
            dataTable.Columns.Add("Value", typeof(string));

            dataTable.Rows.Add(nameof(HarmonySearchType), harmonySearcher.Type.ToString());
            dataTable.Rows.Add(nameof(harmonySearcher.HarmonyGeneratorType), harmonySearcher.HarmonyGeneratorType);
            dataTable.Rows.Add(nameof(harmonySearcher.ObjectiveFunctionType), harmonySearcher.ObjectiveFunctionType);
            dataTable.Rows.Add(nameof(harmonySearcher.MaxImprovisationCount), harmonySearcher.MaxImprovisationCount);
            dataTable.Rows.Add("HarmonyMemorySize",
                harmonySearcher.HarmonyMemory.MaxCapacity);

            switch (harmonySearcher.Type)
            {
                case HarmonySearchType.Standard:
                case HarmonySearchType.Divided:
                    dataTable.Rows.Add(nameof(IParameterProvider.HarmonyMemoryConsiderationRatio), harmonySearcher.ParameterProvider.PitchAdjustmentRatio);
                    dataTable.Rows.Add(nameof(IParameterProvider.PitchAdjustmentRatio), harmonySearcher.ParameterProvider.PitchAdjustmentRatio);
                    break;

                case HarmonySearchType.ImprovedDivided:
                case HarmonySearchType.Improved:
                    var dynamicParProvider = harmonySearcher.ParameterProvider as DynamicPitchAdjustmentRatioProvider;

                    dataTable.Rows.Add(nameof(IParameterProvider.HarmonyMemoryConsiderationRatio), dynamicParProvider.HarmonyMemoryConsiderationRatio);
                    dataTable.Rows.Add($"{nameof(dynamicParProvider.MinPitchAdjustmentRatio)}",
                        dynamicParProvider.MinPitchAdjustmentRatio);
                    dataTable.Rows.Add($"{nameof(dynamicParProvider.MaxPitchAdjustmentRatio)}",
                        dynamicParProvider.MaxPitchAdjustmentRatio);
                    break;

                case HarmonySearchType.Dynamic:
                case HarmonySearchType.DynamicDivided:
                    dataTable.Rows.Add(nameof(IParameterProvider.HarmonyMemoryConsiderationRatio), "Dynamic");
                    dataTable.Rows.Add(nameof(IParameterProvider.PitchAdjustmentRatio), "Dynamic");
                    break;
            }

            return dataTable;
        }

        /// <summary>
        /// Gets the common information data table.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>Data table with common info</returns>
        internal static DataTable GetCommonInfoDataTable(Location source, Location destination)
        {
            var infoDataTable = new DataTable();

            infoDataTable.Columns.Add("Key", typeof(string));
            infoDataTable.Columns.Add("Value", typeof(string));

            infoDataTable.Rows.Add(nameof(DateTime), $"{DateTime.Now:F}");
            infoDataTable.Rows.Add(nameof(source).ToUpper(), $"{source.Name}");
            infoDataTable.Rows.Add(nameof(destination).ToUpper(), $"{destination.Name}");

            return infoDataTable;
        }

        /// <summary>
        /// Gets the test results data table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="testResults">The test results.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>Data table with test results</returns>
        internal static DataTable GetHarmonySearchTestResultsDataTable(string tableName, List<HarmonySearchAverageTestResult> testResults, string typeName)
        {
            var resultsDataTable = new DataTable(tableName);

            if (typeName != nameof(HarmonySearchAverageTestResult.HarmonySearchType))
            {
                resultsDataTable.Columns.Add(nameof(HarmonySearchAverageTestResult.HarmonySearchType), typeof(string));
            }

            if (typeName != nameof(HarmonySearchAverageTestResult.HarmonyGeneratorType))
            {
                resultsDataTable.Columns.Add(nameof(HarmonySearchAverageTestResult.HarmonyGeneratorType), typeof(string));
            }

            if (typeName != nameof(HarmonySearchAverageTestResult.ObjectiveFunctionType))
            {
                resultsDataTable.Columns.Add(nameof(HarmonySearchAverageTestResult.ObjectiveFunctionType), typeof(string));
            }

            resultsDataTable.Columns.Add("Time [ms]", typeof(double));
            resultsDataTable.Columns.Add("Best harmony", typeof(double));
            resultsDataTable.Columns.Add("Average harmony", typeof(double));
            resultsDataTable.Columns.Add("Worst harmony", typeof(double));
            resultsDataTable.Columns.Add(nameof(HarmonySearchAverageTestResult.ImprovisationCount), typeof(int));
            resultsDataTable.Columns.Add(nameof(HarmonySearchAverageTestResult.FeasibleSolutionsCount), typeof(int));
            resultsDataTable.Columns.Add(nameof(HarmonySearchAverageTestResult.NonFeasibleCount), typeof(int));

            foreach (var testResult in testResults)
            {
                var typesDataRow = new List<object>(2);

                if (typeName != nameof(HarmonySearchAverageTestResult.HarmonySearchType))
                {
                    typesDataRow.Add(testResult.HarmonySearchType.ToString());
                }
                if (typeName != nameof(HarmonySearchAverageTestResult.HarmonyGeneratorType))
                {
                    typesDataRow.Add(testResult.HarmonyGeneratorType.ToString());
                }
                if (typeName != nameof(HarmonySearchAverageTestResult.ObjectiveFunctionType))
                {
                    typesDataRow.Add(testResult.ObjectiveFunctionType.ToString());
                }

                var testResultDataRow = new List<object>
                {
                    testResult.AverageTime,
                    testResult.BestObjectiveFunctionValue,
                    testResult.AverageObjectiveFunctionValue,
                    testResult.WorstObjectiveFunctionValue,
                    testResult.ImprovisationCount,
                    testResult.FeasibleSolutionsCount,
                    testResult.NonFeasibleCount
                };

                resultsDataTable.Rows.Add(typesDataRow.Concat(testResultDataRow).ToArray());
            }

            return resultsDataTable;
        }

        /// <summary>
        /// Creates the solutions data table.
        /// </summary>
        /// <param name="testResults"></param>
        /// <returns></returns>
        internal static DataTable GetSolutionsDataTable(List<TestResult> testResults)
        {
            var solutionsDataTable = new DataTable();

            solutionsDataTable.Columns.Add("Id", typeof(int));
            solutionsDataTable.Columns.Add("Best harmony", typeof(double));
            solutionsDataTable.Columns.Add("Time [s]", typeof(double));
            solutionsDataTable.Columns.Add("Arguments", typeof(string));

            for (var index = 0; index < testResults.Count; index++)
            {
                var testResult = testResults[index];
                solutionsDataTable.Rows.Add(index + 1,
                    testResult.Solution.ObjectiveValue,
                    testResult.Time.TotalSeconds,
                    string.Concat(testResult.Solution.Arguments.Select(a => $"{a.ToString()} | ")));
            }

            return solutionsDataTable;
        }

        /// <summary>
        /// Gets the test results data table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="testResults">The test results.</param>
        /// <returns>Data table with test results</returns>
        internal static DataTable GetTestResultsDataTable(string tableName, List<AverageTestResult> testResults)
        {
            var resultsDataTable = new DataTable(tableName);

            resultsDataTable.Columns.Add("Algorithm type", typeof(string));
            resultsDataTable.Columns.Add(nameof(HarmonySearchAverageTestResult.HarmonyGeneratorType), typeof(string));
            resultsDataTable.Columns.Add(nameof(HarmonySearchAverageTestResult.ObjectiveFunctionType), typeof(string));
            resultsDataTable.Columns.Add("Time [s]", typeof(double));
            resultsDataTable.Columns.Add("Best harmony", typeof(double));
            resultsDataTable.Columns.Add("Average harmony", typeof(double));
            resultsDataTable.Columns.Add("Worst harmony", typeof(double));
            resultsDataTable.Columns.Add(nameof(AverageTestResult.FeasibleSolutionsCount), typeof(int));
            resultsDataTable.Columns.Add(nameof(AverageTestResult.NonFeasibleCount), typeof(int));

            foreach (var testResult in testResults)
            {
                var typesDataRow = new List<object>(3);

                if (testResult is HarmonySearchAverageTestResult harmonySearchAverageTestResult)
                {
                    typesDataRow.Add(harmonySearchAverageTestResult.HarmonySearchType.ToString());
                    typesDataRow.Add(harmonySearchAverageTestResult.HarmonyGeneratorType.ToString());
                    typesDataRow.Add(harmonySearchAverageTestResult.ObjectiveFunctionType.ToString());
                }
                else
                {
                    typesDataRow.Add("A*");
                    typesDataRow.Add("—");
                    typesDataRow.Add("—");
                }

                var testResultDataRow = new List<object>
                {
                    testResult.AverageTime,
                    testResult.BestObjectiveFunctionValue,
                    testResult.AverageObjectiveFunctionValue,
                    testResult.WorstObjectiveFunctionValue,
                    testResult.FeasibleSolutionsCount,
                    testResult.NonFeasibleCount
                };

                resultsDataTable.Rows.Add(typesDataRow.Concat(testResultDataRow).ToArray());
            }

            return resultsDataTable;
        }
    }
}