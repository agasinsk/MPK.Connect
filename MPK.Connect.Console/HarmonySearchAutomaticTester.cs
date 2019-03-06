using System;
using System.Data;
using System.Linq;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Experiment;
using MPK.Connect.Service.Export;

namespace MPK.Connect.Console
{
    internal class HarmonySearchAutomaticTester<T> where T : class
    {
        private readonly IActionTimer _actionTimer;
        private readonly IExporterService _exporterService;

        public HarmonySearchAutomaticTester(IExporterService exporterService, IActionTimer measurable)
        {
            _exporterService = exporterService ?? throw new ArgumentNullException(nameof(exporterService));
            _actionTimer = measurable ?? throw new ArgumentNullException(nameof(measurable));
        }

        public void RunTests(IHarmonySearcher<T> harmonySearcher, Location source, Location destination)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Property", typeof(string));
            dataTable.Columns.Add("Value", typeof(string));

            dataTable.Rows.Add($"{nameof(DateTime)}", DateTime.Now);
            dataTable.Rows.Add($"Source", source.ToString());
            dataTable.Rows.Add($"Destination", destination.ToString());
            dataTable.Rows.Add($"{nameof(harmonySearcher.HarmonyMemoryConsiderationRatio)}",
                harmonySearcher.HarmonyMemoryConsiderationRatio);
            dataTable.Rows.Add($"{nameof(harmonySearcher.MaxImprovisationCount)}", harmonySearcher.MaxImprovisationCount);
            dataTable.Rows.Add($"{nameof(harmonySearcher.HarmonyMemory.MaxCapacity)}",
                harmonySearcher.HarmonyMemory.MaxCapacity);
            dataTable.Rows.Add($"{nameof(harmonySearcher.ShouldImprovePitchAdjustingScenario)}",
                harmonySearcher.ShouldImprovePitchAdjustingScenario.ToString());
            dataTable.Rows.Add($"{nameof(harmonySearcher.PitchAdjustmentRatio)}", harmonySearcher.PitchAdjustmentRatio);
            dataTable.Rows.Add($"{nameof(harmonySearcher.MinPitchAdjustmentRatio)}",
                harmonySearcher.MinPitchAdjustmentRatio);
            dataTable.Rows.Add($"{nameof(harmonySearcher.MaxPitchAdjustmentRatio)}",
                harmonySearcher.MaxPitchAdjustmentRatio);

            var solutionsDataTable = new DataTable();
            solutionsDataTable.Columns.Add("Id", typeof(int));
            solutionsDataTable.Columns.Add("Best harmony", typeof(double));
            solutionsDataTable.Columns.Add("Arguments", typeof(string));
            solutionsDataTable.Columns.Add("Time", typeof(TimeSpan));

            for (int i = 1; i <= 15; i++)
            {
                Harmony<T> bestHarmony = null;
                var elapsed = _actionTimer.MeasureTime(() => { bestHarmony = harmonySearcher.SearchForHarmony(); });

                solutionsDataTable.Rows.Add(i, bestHarmony.ObjectiveValue,
                    string.Concat(bestHarmony.Arguments.Select(a => $"{a.ToString()}\n")), elapsed);
            }

            _exporterService.ExportToExcel(dataTable, solutionsDataTable);
        }
    }
}