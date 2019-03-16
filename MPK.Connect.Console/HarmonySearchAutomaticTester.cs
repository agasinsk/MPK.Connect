using System;
using System.Data;
using System.IO;
using System.Linq;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Business.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Experiment;
using MPK.Connect.Service.Export;

namespace MPK.Connect.Console
{
    internal class HarmonySearchAutomaticTester<T> where T : class
    {
        private readonly IActionTimer _actionTimer;
        private readonly IExporterService _exporterService;
        private readonly IGraphBuilder _graphBuilder;
        private Location _destination;
        private Location _source;

        public HarmonySearchAutomaticTester(IExporterService exporterService, IActionTimer actionTimer, IGraphBuilder graphBuilder)
        {
            _exporterService = exporterService ?? throw new ArgumentNullException(nameof(exporterService));
            _actionTimer = actionTimer ?? throw new ArgumentNullException(nameof(actionTimer));
            _graphBuilder = graphBuilder ?? throw new ArgumentNullException(nameof(graphBuilder));
        }

        public void RunTests(IHarmonySearcher<T> harmonySearcher, string resultPath)
        {
            var dataTable = GetParameterDataTable(harmonySearcher);
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
                System.Console.WriteLine($"Finished testing {harmonySearcher.Type.ToString()}, with {harmonySearcher.GetObjectiveFunctionType().Name}, iteration {i}.");
            }

            var filePath = Path.Combine(resultPath, $"{harmonySearcher.Type.ToString()}_TestResults");
            _exporterService.ExportToExcel(dataTable, solutionsDataTable, filePath);
        }

        public void RunTestsWithScenarios(HarmonySearchTestScenarios<T> scenarios, Location source, Location destination)
        {
            _source = source;
            _destination = destination;

            var graph = _graphBuilder.GetGraph(DateTime.Now);

            var resultPath = $"Tests_{DateTime.Now:ddMMyyyy_HHmm}";

            foreach (var harmonySearchTestSettings in scenarios.Settings)
            {
                var harmonySearcher = harmonySearchTestSettings.GetHarmonySearcher(graph, source, destination);
                RunTests(harmonySearcher, resultPath);
            }
        }

        private DataTable GetParameterDataTable(IHarmonySearcher<T> harmonySearcher)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Property", typeof(string));
            dataTable.Columns.Add("Value", typeof(string));

            dataTable.Rows.Add($"{nameof(DateTime)}", DateTime.Now);
            dataTable.Rows.Add("Source", _source.ToString());
            dataTable.Rows.Add("Destination", _destination.ToString());
            dataTable.Rows.Add($"{nameof(Type)}", harmonySearcher.Type.ToString());
            dataTable.Rows.Add("ObjectiveFunction", harmonySearcher.GetObjectiveFunctionType().Name.TrimEnd('`', '1'));
            dataTable.Rows.Add($"{nameof(harmonySearcher.HarmonyMemoryConsiderationRatio)}",
                harmonySearcher.HarmonyMemoryConsiderationRatio);
            dataTable.Rows.Add($"{nameof(harmonySearcher.MaxImprovisationCount)}", harmonySearcher.MaxImprovisationCount);
            dataTable.Rows.Add($"{nameof(harmonySearcher.HarmonyMemory.MaxCapacity)}",
                harmonySearcher.HarmonyMemory.MaxCapacity);
            dataTable.Rows.Add($"{nameof(harmonySearcher.PitchAdjustmentRatio)}", harmonySearcher.PitchAdjustmentRatio);

            return dataTable;
        }

        private DataTable GetSolutionsDataTable()
        {
            var solutionsDataTable = new DataTable();
            solutionsDataTable.Columns.Add("Id", typeof(int));
            solutionsDataTable.Columns.Add("Best harmony", typeof(double));
            solutionsDataTable.Columns.Add("Arguments", typeof(string));
            solutionsDataTable.Columns.Add("Time", typeof(TimeSpan));
            return solutionsDataTable;
        }
    }
}