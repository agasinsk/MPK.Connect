using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Business.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Generator;
using MPK.Connect.Service.Helpers;

namespace MPK.Connect.Console
{
    internal class HarmonySearchStopTimeTester
    {
        private readonly IActionTimer _actionTimer;
        private readonly IExcelExporterService _excelExporterService;
        private readonly IGraphBuilder _graphBuilder;

        public HarmonySearchStopTimeTester(IExcelExporterService excelExporterService, IActionTimer actionTimer, IGraphBuilder graphBuilder)
        {
            _excelExporterService = excelExporterService ?? throw new ArgumentNullException(nameof(excelExporterService));
            _actionTimer = actionTimer ?? throw new ArgumentNullException(nameof(actionTimer));
            _graphBuilder = graphBuilder ?? throw new ArgumentNullException(nameof(graphBuilder));
        }

        public void RunTestsWithScenarios(HarmonySearchTestScenario scenario, Location source, Location destination)
        {
            var graph = _graphBuilder.GetGraph(DateTime.Now);

            var resultDirectory = $"Tests_{DateTime.Now:ddMMyyyy_HHmm}_{source.Name.Trim()}_{destination.Name.Trim()}";

            var infoDataTable = GetInfoDataTable(source, destination);

            var averageResults = new Dictionary<HarmonyGeneratorType, List<TestResult>>();

            foreach (var harmonySearchTestSettings in scenario.Settings)
            {
                var harmonySearcher = harmonySearchTestSettings.GetHarmonySearcher(graph, source, destination);
                var testResult = RunTests(harmonySearcher, infoDataTable, resultDirectory);

                if (!averageResults.ContainsKey(harmonySearcher.HarmonyGeneratorType))
                {
                    averageResults.Add(harmonySearcher.HarmonyGeneratorType, new List<TestResult>());
                }

                averageResults[harmonySearcher.HarmonyGeneratorType].Add(testResult);
            }

            ExportResults(averageResults, infoDataTable, resultDirectory);
        }

        private void ExportResults(Dictionary<HarmonyGeneratorType, List<TestResult>> results, DataTable infoDataTable, string resultDirectory)
        {
            var dataTables = new List<DataTable>();
            foreach (var (functionType, testResults) in results)
            {
                var resultsDataTable = GetResultsDataTable(functionType.ToString());

                foreach (var testResult in testResults)
                {
                    resultsDataTable.Rows.Add(testResult.GetDataRowParamsWithoutFunctionType());
                }

                dataTables.Add(resultsDataTable);
            }

            var filePath = Path.Combine(resultDirectory, "AverageTestResults");
            _excelExporterService.ExportToExcel(infoDataTable, dataTables, filePath);
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

        private DataTable GetResultsDataTable(string tableName)
        {
            var objectiveFunctionDataTable = new DataTable(tableName);

            objectiveFunctionDataTable.Columns.Add(nameof(TestResult.HarmonySearchType), typeof(string));
            objectiveFunctionDataTable.Columns.Add(nameof(TestResult.SolutionsCount), typeof(int));
            objectiveFunctionDataTable.Columns.Add(nameof(TestResult.NonFeasibleCount), typeof(int));
            objectiveFunctionDataTable.Columns.Add("Best harmony", typeof(double));
            objectiveFunctionDataTable.Columns.Add("Time [ms]", typeof(double));

            return objectiveFunctionDataTable;
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

        private TestResult RunTests(IHarmonySearcher<StopTimeInfo> harmonySearcher, DataTable infoDataTable, string resultPath)
        {
            var dataTable = harmonySearcher.ToDataTable();
            var solutionsDataTable = GetSolutionsDataTable();

            var elapsedTimes = new List<TimeSpan>();
            var objectiveFunctionValues = new List<double>();

            for (var i = 1; i <= 15; i++)
            {
                Harmony<StopTimeInfo> bestHarmony = null;
                var elapsed = _actionTimer.MeasureTime(() =>
                {
                    bestHarmony = harmonySearcher.SearchForHarmony();
                });

                elapsedTimes.Add(elapsed);
                objectiveFunctionValues.Add(bestHarmony.ObjectiveValue);

                solutionsDataTable.Rows.Add(i, bestHarmony.ObjectiveValue,
                    string.Concat(bestHarmony.Arguments.Select(a => $" {a.ToString()} |")), elapsed.TotalSeconds);
                System.Console.WriteLine($"Finished testing {harmonySearcher.Type.ToString()} HS, with function {harmonySearcher.HarmonyGeneratorType}, iteration {i}.");
            }

            var filePath = Path.Combine(resultPath, $"{harmonySearcher.Type}_{harmonySearcher.HarmonyGeneratorType}_TestResults");
            _excelExporterService.ExportToExcel(infoDataTable, dataTable, solutionsDataTable, filePath);
            System.Console.WriteLine($"Saved file under path: {filePath}");

            return new TestResult
            {
                HarmonySearchType = harmonySearcher.Type,
                HarmonyGeneratorType = harmonySearcher.HarmonyGeneratorType,
                ObjectiveFunctionValue = objectiveFunctionValues.All(double.IsPositiveInfinity) ? double.PositiveInfinity : objectiveFunctionValues.Where(d => !double.IsPositiveInfinity(d)).Average(),
                Time = elapsedTimes.Select(t => t.TotalSeconds).Average(),
                NonFeasibleCount = objectiveFunctionValues.Count(double.IsPositiveInfinity),
                SolutionsCount = objectiveFunctionValues.Count
            };
        }
    }
}