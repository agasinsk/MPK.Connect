using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Microsoft.Extensions.Logging;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Helpers;
using MPK.Connect.TestEnvironment.Helpers;
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
        private readonly ILogger<HarmonySearchStopTimeTester> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="excelExportService">Excel export service</param>
        /// <param name="actionTimer">Action timer</param>
        /// <param name="graphBuilder">Graph builder</param>
        /// <param name="logger">The logger</param>
        public HarmonySearchStopTimeTester(IExcelExportService excelExportService, IActionTimer actionTimer, IGraphBuilder graphBuilder, ILogger<HarmonySearchStopTimeTester> logger)
        {
            _excelExportService = excelExportService ?? throw new ArgumentNullException(nameof(excelExportService));
            _actionTimer = actionTimer ?? throw new ArgumentNullException(nameof(actionTimer));
            _graphBuilder = graphBuilder ?? throw new ArgumentNullException(nameof(graphBuilder));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Runs the harmony search tests with scenario and locations.
        /// </summary>
        /// <param name="locations">The locations.</param>
        /// <param name="scenario">The test scenario.</param>
        /// <param name="dateTime">The date time.</param>
        public void RunTestsWithLocations(List<Tuple<Location, Location>> locations, HarmonySearchTestScenario scenario, DateTime? dateTime = null)
        {
            foreach (var (source, destination) in locations)
            {
                RunTestsWithScenario(scenario, source, destination, dateTime);

                _logger.LogInformation($"Finished testing Harmony Search: {source.Name} --> {destination.Name}");
            }
        }

        /// <summary>
        /// Runs the tests with scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="dateTime">The date time.</param>
        public void RunTestsWithScenario(HarmonySearchTestScenario scenario, Location source, Location destination, DateTime? dateTime = null)
        {
            var graph = _graphBuilder.GetGraph(dateTime ?? DateTime.Now);

            var topResultDirectory = $"Tests_{DateTime.Now:ddMMyyyy_HHmm}_{source.Name.Trim()}_{destination.Name.Trim()}";

            var infoDataTable = DataTableUtils.GetCommonInfoDataTable(source, destination);
            var averageTestResults = RunTests(scenario, source, destination, graph, topResultDirectory, infoDataTable);

            ExportAverageTestResults(averageTestResults, infoDataTable, topResultDirectory);
        }

        /// <summary>
        /// Exports the average test results.
        /// </summary>
        /// <param name="testResults">The test results.</param>
        /// <param name="commonInfoDataTable">The common information data table.</param>
        /// <param name="outputDirectory">The output directory.</param>
        private void ExportAverageTestResults(List<HarmonySearchAverageTestResult> testResults, DataTable commonInfoDataTable, string outputDirectory)
        {
            // Group results by type
            var groupedTestResults = TestResultUtils.GroupTestResults(testResults);

            var resultDataTables = new Dictionary<string, List<DataTable>>();

            foreach (var (typeName, groupedResults) in groupedTestResults)
            {
                var groupDataTables = new List<DataTable>();

                foreach (var (groupKey, groupResults) in groupedResults)
                {
                    var dataTable = DataTableUtils.GetTestResultsDataTable(groupKey, groupResults, typeName);
                    groupDataTables.Add(dataTable);
                }

                resultDataTables[typeName] = groupDataTables;
            }

            // Export average result to Excel
            var filePath = Path.Combine(outputDirectory, "AverageTestResults");

            _excelExportService.ExportToExcel(commonInfoDataTable, resultDataTables, filePath);
        }

        /// <summary>
        /// Runs the single HS test.
        /// </summary>
        /// <param name="harmonySearcher">The harmony searcher.</param>
        /// <returns>Test result</returns>
        private HarmonySearchTestResult RunSingleTest(IHarmonySearcher<StopTimeInfo> harmonySearcher)
        {
            Harmony<StopTimeInfo> bestHarmony = null;
            var elapsedTime = _actionTimer.MeasureTime(() =>
            {
                bestHarmony = harmonySearcher.SearchForHarmony();
            });

            return new HarmonySearchTestResult
            {
                HarmonySearchType = harmonySearcher.Type,
                ObjectiveFunctionType = harmonySearcher.ObjectiveFunctionType,
                HarmonyGeneratorType = harmonySearcher.HarmonyGeneratorType,
                BestHarmony = bestHarmony,
                Time = elapsedTime,
                ImprovisationCount = harmonySearcher.ImprovisationCount
            };
        }

        /// <summary>
        /// Runs the tests.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="graph">The graph.</param>
        /// <param name="topResultDirectory">The top result directory.</param>
        /// <param name="infoDataTable">The information data table.</param>
        /// <returns></returns>
        private List<HarmonySearchAverageTestResult> RunTests(HarmonySearchTestScenario scenario, Location source, Location destination, Graph<int, StopTimeInfo> graph, string topResultDirectory, DataTable infoDataTable)
        {
            var averageTestResults = new List<HarmonySearchAverageTestResult>();

            foreach (var harmonySearchTestSettings in scenario.Settings)
            {
                var harmonySearcher = harmonySearchTestSettings.GetHarmonySearcher(graph, source, destination);

                var testsResult = RunTests(harmonySearcher, infoDataTable, topResultDirectory);

                averageTestResults.Add(testsResult);
            }

            return averageTestResults;
        }

        /// <summary>
        /// Runs the tests.
        /// </summary>
        /// <param name="harmonySearcher">The harmony searcher.</param>
        /// <param name="infoDataTable">The information data table.</param>
        /// <param name="resultPath">The result path.</param>
        /// <returns>Tests result</returns>
        private HarmonySearchAverageTestResult RunTests(IHarmonySearcher<StopTimeInfo> harmonySearcher, DataTable infoDataTable, string resultPath)
        {
            var testResults = new List<HarmonySearchTestResult>(15);

            for (var iteration = 1; iteration <= 15; iteration++)
            {
                var singleIterationTestResult = RunSingleTest(harmonySearcher);
                testResults.Add(singleIterationTestResult);

                _logger.LogInformation($"Finished testing {harmonySearcher.Type.ToString()} HS, generator {harmonySearcher.HarmonyGeneratorType}, function {harmonySearcher.ObjectiveFunctionType}, iteration {iteration}.");
            }

            var parameterDataTable = DataTableUtils.GetParameterDataTable(harmonySearcher);
            var solutionsDataTable = DataTableUtils.GetSolutionsDataTable(testResults);

            // Export test results to file
            var filePath = Path.Combine(resultPath, "details", $"{harmonySearcher.Type}_{harmonySearcher.HarmonyGeneratorType}_{harmonySearcher.ObjectiveFunctionType}_TestResults");

            _excelExportService.ExportToExcel(infoDataTable, parameterDataTable, solutionsDataTable, filePath);

            _logger.LogInformation($"Saved file under path: {filePath}");

            // Return the average results
            return new HarmonySearchAverageTestResult(harmonySearcher.Type, harmonySearcher.HarmonyGeneratorType, harmonySearcher.ObjectiveFunctionType, testResults);
        }
    }
}