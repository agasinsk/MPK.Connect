using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Business.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Generator;
using MPK.Connect.Service.Helpers;
using MPK.Connect.TestEnvironment.Settings;

namespace MPK.Connect.TestEnvironment
{
    /// <summary>
    /// Harmony search tester
    /// </summary>
    internal class HarmonySearchStopTimeTester
    {
        private readonly IActionTimer _actionTimer;
        private readonly IExcelExportService _excelExportService;
        private readonly IGraphBuilder _graphBuilder;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="excelExportService">Excel export service</param>
        /// <param name="actionTimer">Action timer</param>
        /// <param name="graphBuilder">Graph builder</param>
        public HarmonySearchStopTimeTester(IExcelExportService excelExportService, IActionTimer actionTimer, IGraphBuilder graphBuilder)
        {
            _excelExportService = excelExportService ?? throw new ArgumentNullException(nameof(excelExportService));
            _actionTimer = actionTimer ?? throw new ArgumentNullException(nameof(actionTimer));
            _graphBuilder = graphBuilder ?? throw new ArgumentNullException(nameof(graphBuilder));
        }

        /// <summary>
        /// Runs test with scenarios
        /// </summary>
        /// <param name="scenario"></param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public void RunTestsWithScenarios(HarmonySearchTestScenario scenario, Location source, Location destination)
        {
            var graph = _graphBuilder.GetGraph(DateTime.Now);

            var topResultDirectory = $"Tests_{DateTime.Now:ddMMyyyy_HHmm}_{source.Name.Trim()}_{destination.Name.Trim()}";

            var infoDataTable = GetInfoDataTable(source, destination);

            var averageTestResults = new List<HarmonySearchTestResult>();

            foreach (var harmonySearchTestSettings in scenario.Settings)
            {
                var harmonySearcher = harmonySearchTestSettings.GetHarmonySearcher(graph, source, destination);

                var testsResult = RunTests(harmonySearcher, infoDataTable, topResultDirectory);

                averageTestResults.Add(testsResult);
            }

            ExportAverageResults(averageTestResults, infoDataTable, topResultDirectory);
        }

        private void ExportAverageResults(List<HarmonySearchTestResult> results, DataTable infoDataTable, string resultDirectory)
        {
            var groupedTestResults = GetGroupedTestResults(results);

            var resultDataTables = new Dictionary<string, List<DataTable>>();

            foreach (var (typeName, groupedResults) in groupedTestResults)
            {
                var dataTables = new List<DataTable>();

                foreach (var (groupKey, groupResults) in groupedResults)
                {
                    var dataTable = GetResultsDataTable(groupKey, groupResults, typeName);
                    dataTables.Add(dataTable);
                }

                resultDataTables[typeName] = dataTables;
            }

            var filePath = Path.Combine(resultDirectory, "AverageTestResults");

            _excelExportService.ExportToExcel(infoDataTable, resultDataTables, filePath);
        }

        private Dictionary<string, Dictionary<string, List<HarmonySearchTestResult>>> GetGroupedTestResults(List<HarmonySearchTestResult> results)
        {
            return new Dictionary<string, Dictionary<string, List<HarmonySearchTestResult>>>
            {
                [nameof(HarmonySearchType)] = results
                    .GroupBy(r => r.HarmonySearchType)
                    .ToDictionary(k => k.Key.ToString(), v => v.ToList()),

                [nameof(HarmonyGeneratorType)] = results
                    .GroupBy(r => r.HarmonyGeneratorType)
                    .ToDictionary(k => k.Key.ToString(), v => v.ToList()),

                [nameof(ObjectiveFunctionType)] = results
                    .GroupBy(r => r.ObjectiveFunctionType)
                    .ToDictionary(k => k.Key.ToString(), v => v.ToList()),
            };
        }

        private DataTable GetInfoDataTable(Location source, Location destination)
        {
            var infoDataTable = new DataTable();

            infoDataTable.Columns.Add("Key", typeof(string));
            infoDataTable.Columns.Add("Value", typeof(string));

            infoDataTable.Rows.Add(nameof(DateTime), $"{DateTime.Now:F}");
            infoDataTable.Rows.Add(nameof(source).ToUpper(), $"{source.Name}");
            infoDataTable.Rows.Add(nameof(destination).ToUpper(), $"{destination.Name}");

            return infoDataTable;
        }

        private DataTable GetResultsDataTable(string tableName, List<HarmonySearchTestResult> testResults,
            string typeName)
        {
            var resultsDataTable = new DataTable(tableName);

            if (typeName != nameof(HarmonySearchTestResult.HarmonySearchType))
            {
                resultsDataTable.Columns.Add(nameof(HarmonySearchTestResult.HarmonySearchType), typeof(string));
            }

            if (typeName != nameof(HarmonySearchTestResult.HarmonyGeneratorType))
            {
                resultsDataTable.Columns.Add(nameof(HarmonySearchTestResult.HarmonyGeneratorType), typeof(string));
            }

            if (typeName != nameof(HarmonySearchTestResult.ObjectiveFunctionType))
            {
                resultsDataTable.Columns.Add(nameof(HarmonySearchTestResult.ObjectiveFunctionType), typeof(string));
            }

            resultsDataTable.Columns.Add("Time [ms]", typeof(double));
            resultsDataTable.Columns.Add("Best harmony", typeof(double));
            resultsDataTable.Columns.Add("Average harmony", typeof(double));
            resultsDataTable.Columns.Add("Worst harmony", typeof(double));
            resultsDataTable.Columns.Add(nameof(HarmonySearchTestResult.ImprovisationCount), typeof(int));
            resultsDataTable.Columns.Add(nameof(HarmonySearchTestResult.SolutionsCount), typeof(int));
            resultsDataTable.Columns.Add(nameof(HarmonySearchTestResult.NonFeasibleCount), typeof(int));

            foreach (var testResult in testResults)
            {
                var typesDataRow = new List<object>(2);

                if (typeName != nameof(HarmonySearchTestResult.HarmonySearchType))
                {
                    typesDataRow.Add(testResult.HarmonySearchType.ToString());
                }
                if (typeName != nameof(HarmonySearchTestResult.HarmonyGeneratorType))
                {
                    typesDataRow.Add(testResult.HarmonyGeneratorType.ToString());
                }
                if (typeName != nameof(HarmonySearchTestResult.ObjectiveFunctionType))
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
                    testResult.SolutionsCount,
                    testResult.NonFeasibleCount
                };

                resultsDataTable.Rows.Add(typesDataRow.Concat(testResultDataRow).ToArray());
            }

            return resultsDataTable;
        }

        private DataTable GetSolutionsDataTable()
        {
            var solutionsDataTable = new DataTable();

            solutionsDataTable.Columns.Add("Id", typeof(int));
            solutionsDataTable.Columns.Add("Best harmony", typeof(double));
            solutionsDataTable.Columns.Add("Arguments", typeof(string));
            solutionsDataTable.Columns.Add("Time [s]", typeof(double));

            return solutionsDataTable;
        }

        private HarmonySearchTestResult RunTests(IHarmonySearcher<StopTimeInfo> harmonySearcher, DataTable infoDataTable, string resultPath)
        {
            var parameterDataTable = harmonySearcher.ToDataTable();
            var solutionsDataTable = GetSolutionsDataTable();

            var elapsedTimes = new List<TimeSpan>();
            var objectiveFunctionValues = new List<double>();
            var improvisationCounts = new List<int>();

            for (var i = 1; i <= 15; i++)
            {
                Harmony<StopTimeInfo> bestHarmony = null;
                var elapsedTime = _actionTimer.MeasureTime(() =>
                {
                    bestHarmony = harmonySearcher.SearchForHarmony();
                });

                elapsedTimes.Add(elapsedTime);
                objectiveFunctionValues.Add(bestHarmony.ObjectiveValue);
                improvisationCounts.Add(harmonySearcher.ImprovisationCount);

                solutionsDataTable.Rows.Add(i, bestHarmony.ObjectiveValue,
                    string.Concat(bestHarmony.Arguments.Select(a => $"{a.ToString()} | ")), elapsedTime.TotalSeconds);

                Console.WriteLine($"Finished testing {harmonySearcher.Type.ToString()} HS, generator {harmonySearcher.HarmonyGeneratorType}, function {harmonySearcher.ObjectiveFunctionType}, iteration {i}.");
            }

            var filePath = Path.Combine(resultPath, $"{harmonySearcher.Type}_{harmonySearcher.HarmonyGeneratorType}_{harmonySearcher.ObjectiveFunctionType}_TestResults");

            _excelExportService.ExportToExcel(infoDataTable, parameterDataTable, solutionsDataTable, filePath);

            Console.WriteLine($"Saved file under path: {filePath}");

            return new HarmonySearchTestResult
            {
                HarmonySearchType = harmonySearcher.Type,
                ObjectiveFunctionType = harmonySearcher.ObjectiveFunctionType,
                HarmonyGeneratorType = harmonySearcher.HarmonyGeneratorType,
                BestObjectiveFunctionValue = objectiveFunctionValues.Min(),
                WorstObjectiveFunctionValue = objectiveFunctionValues.Max(),
                AverageObjectiveFunctionValue = objectiveFunctionValues.All(double.IsPositiveInfinity) ? double.PositiveInfinity : objectiveFunctionValues.Where(d => !double.IsPositiveInfinity(d)).Average(),
                AverageTime = elapsedTimes.Select(t => t.TotalSeconds).Average(),
                NonFeasibleCount = objectiveFunctionValues.Count(double.IsPositiveInfinity),
                ImprovisationCount = (int)Math.Ceiling(improvisationCounts.Average()),
                SolutionsCount = objectiveFunctionValues.Count
            };
        }
    }
}