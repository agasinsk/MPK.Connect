using System;
using System.Data;
using System.IO;
using System.Linq;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Business.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Core;
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

            var resultPath = $"Tests_{DateTime.Now:ddMMyyyy_HHmm}";

            foreach (var harmonySearchTestSettings in scenarios.Settings)
            {
                var harmonySearcher = harmonySearchTestSettings.GetHarmonySearcher(graph, source, destination);
                RunTests(harmonySearcher, resultPath);
            }
        }

        private DataTable GetSolutionsDataTable()
        {
            var solutionsDataTable = new DataTable();

            solutionsDataTable.Columns.Add("Id", typeof(int));
            solutionsDataTable.Columns.Add("Best harmony", typeof(double));
            solutionsDataTable.Columns.Add("Arguments", typeof(string));
            solutionsDataTable.Columns.Add("Time [s]", typeof(TimeSpan));

            return solutionsDataTable;
        }

        private void RunTests(IHarmonySearcher<T> harmonySearcher, string resultPath)
        {
            var dataTable = harmonySearcher.ToDataTable();
            var solutionsDataTable = GetSolutionsDataTable();

            for (var i = 1; i <= 15; i++)
            {
                Harmony<T> bestHarmony = null;
                var elapsed = _actionTimer.MeasureTime(() =>
                {
                    bestHarmony = harmonySearcher.SearchForHarmony();
                });

                solutionsDataTable.Rows.Add(i, bestHarmony.ObjectiveValue,
                    string.Concat(bestHarmony.Arguments.Select(a => $" {a.ToString()} |")), elapsed);
                System.Console.WriteLine($"Finished testing {harmonySearcher.Type.ToString()}, with {harmonySearcher.ObjectiveFunctionType}, iteration {i}.");
            }

            var filePath = Path.Combine(resultPath, $"{harmonySearcher.Type}_{harmonySearcher.ObjectiveFunctionType}_TestResults");
            _excelExporterService.ExportToExcel(dataTable, solutionsDataTable, filePath);
        }
    }
}