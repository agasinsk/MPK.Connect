using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Business.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Helpers;

namespace MPK.Connect.Console
{
    internal class HarmonySearchAutomaticTester<T> where T : class
    {
        private readonly IActionTimer _actionTimer;
        private readonly IExcelExporterService _excelExporterService;
        private readonly IGraphBuilder _graphBuilder;

        public HarmonySearchAutomaticTester(IExcelExporterService excelExporterService, IActionTimer actionTimer, IGraphBuilder graphBuilder)
        {
            _excelExporterService = excelExporterService ?? throw new ArgumentNullException(nameof(excelExporterService));
            _actionTimer = actionTimer ?? throw new ArgumentNullException(nameof(actionTimer));
            _graphBuilder = graphBuilder ?? throw new ArgumentNullException(nameof(graphBuilder));
        }

        public void RunTestsWithScenarios(HarmonySearchTestScenarios<T> scenarios, Location source, Location destination)
        {
            var graph = _graphBuilder.GetGraph(DateTime.Now);

            var resultDirectory = $"Tests_{DateTime.Now:ddMMyyyy_HHmm}";

            var averageResults = new Dictionary<ObjectiveFunctionType, List<TestResult>>();

            foreach (var harmonySearchTestSettings in scenarios.Settings)
            {
                var harmonySearcher = harmonySearchTestSettings.GetHarmonySearcher(graph, source, destination);
                var testResult = RunTests(harmonySearcher, source, destination, resultDirectory);

                if (!averageResults.ContainsKey(harmonySearcher.ObjectiveFunctionType))
                {
                    averageResults.Add(harmonySearcher.ObjectiveFunctionType, new List<TestResult>());
                }

                averageResults[harmonySearcher.ObjectiveFunctionType].Add(testResult);
            }

            ExportResults(averageResults, resultDirectory);
        }

        private void ExportResults(Dictionary<ObjectiveFunctionType, List<TestResult>> results, string resultDirectory)
        {
            var dataTables = new List<DataTable>();
            foreach (var resultGroup in results)
            {
                var objectiveFunctionDataTable = new DataTable(resultGroup.Key.ToString());

                objectiveFunctionDataTable.Columns.Add(nameof(TestResult.HarmonySearchType), typeof(string));
                objectiveFunctionDataTable.Columns.Add("Best harmony", typeof(double));
                objectiveFunctionDataTable.Columns.Add("Time [ms]", typeof(double));

                foreach (var testResult in resultGroup.Value)
                {
                    objectiveFunctionDataTable.Rows.Add(testResult.GetDataRowParamsWithoutFunctionType());
                }

                dataTables.Add(objectiveFunctionDataTable);
            }

            var filePath = Path.Combine(resultDirectory, "AverageTestResults");
            _excelExporterService.ExportToExcel(dataTables, filePath);
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

        private DataTable GetSolutionsDataTable()
        {
            var solutionsDataTable = new DataTable();

            solutionsDataTable.Columns.Add("Id", typeof(int));
            solutionsDataTable.Columns.Add("Best harmony", typeof(double));
            solutionsDataTable.Columns.Add("Arguments", typeof(string));
            solutionsDataTable.Columns.Add("Time [ms]", typeof(double));

            return solutionsDataTable;
        }

        private TestResult RunTests(IHarmonySearcher<T> harmonySearcher, Location source, Location destination, string resultPath)
        {
            var infoDataTable = GetInfoDataTable(source, destination);
            var dataTable = harmonySearcher.ToDataTable();
            var solutionsDataTable = GetSolutionsDataTable();

            var elapsedTimes = new List<TimeSpan>();
            var objectiveFunctionValues = new List<double>();

            for (var i = 1; i <= 15; i++)
            {
                Harmony<T> bestHarmony = null;
                var elapsed = _actionTimer.MeasureTime(() =>
                {
                    bestHarmony = harmonySearcher.SearchForHarmony();
                });

                elapsedTimes.Add(elapsed);
                objectiveFunctionValues.Add(bestHarmony.ObjectiveValue);

                solutionsDataTable.Rows.Add(i, bestHarmony.ObjectiveValue,
                    string.Concat(bestHarmony.Arguments.Select(a => $" {a.ToString()} |")), elapsed.TotalMilliseconds);
                System.Console.WriteLine($"Finished testing {harmonySearcher.Type.ToString()}, with {harmonySearcher.ObjectiveFunctionType}, iteration {i}.");
            }

            var filePath = Path.Combine(resultPath, $"{harmonySearcher.Type}_{harmonySearcher.ObjectiveFunctionType}_TestResults");
            _excelExporterService.ExportToExcel(infoDataTable, dataTable, solutionsDataTable, filePath);

            return new TestResult
            {
                HarmonySearchType = harmonySearcher.Type,
                ObjectiveFunctionType = harmonySearcher.ObjectiveFunctionType,
                ObjectiveFunctionValue = objectiveFunctionValues.Average(),
                Time = elapsedTimes.Select(t => t.TotalMilliseconds).Average()
            };
        }
    }
}