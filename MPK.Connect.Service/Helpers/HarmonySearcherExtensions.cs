using System;
using System.Data;
using MPK.Connect.Service.Business.HarmonySearch.Core;

namespace MPK.Connect.Service.Helpers
{
    public static class HarmonySearcherExtensions
    {
        public static DataTable ToDataTable<T>(this IHarmonySearcher<T> harmonySearcher)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Property", typeof(string));
            dataTable.Columns.Add("Value", typeof(string));

            dataTable.Rows.Add($"{nameof(DateTime)}", DateTime.Now);
            dataTable.Rows.Add($"{nameof(Type)}", harmonySearcher.Type.ToString());
            dataTable.Rows.Add("ObjectiveFunction", harmonySearcher.ObjectiveFunctionType);
            dataTable.Rows.Add($"{nameof(harmonySearcher.HarmonyMemoryConsiderationRatio)}",
                harmonySearcher.HarmonyMemoryConsiderationRatio);
            dataTable.Rows.Add($"HarmonyMemorySize", harmonySearcher.MaxImprovisationCount);
            dataTable.Rows.Add($"{nameof(harmonySearcher.HarmonyMemory.MaxCapacity)}",
                harmonySearcher.HarmonyMemory.MaxCapacity);

            switch (harmonySearcher.Type)
            {
                case HarmonySearchType.Standard:
                case HarmonySearchType.Divided:
                    dataTable.Rows.Add($"{nameof(harmonySearcher.PitchAdjustmentRatio)}", harmonySearcher.PitchAdjustmentRatio);
                    break;

                case HarmonySearchType.ImprovedDivided:
                case HarmonySearchType.Improved:
                    var improvedHarmonySearcher = harmonySearcher as IImprovedHarmonySearcher<T>;
                    dataTable.Rows.Add($"{nameof(improvedHarmonySearcher.MinPitchAdjustmentRatio)}", improvedHarmonySearcher.MinPitchAdjustmentRatio);
                    dataTable.Rows.Add($"{nameof(improvedHarmonySearcher.MaxPitchAdjustmentRatio)}", improvedHarmonySearcher.MaxPitchAdjustmentRatio);
                    break;

                case HarmonySearchType.Dynamic:
                case HarmonySearchType.DynamicDivided:
                    break;

                default:
                    dataTable.Rows.Add($"{nameof(harmonySearcher.PitchAdjustmentRatio)}", harmonySearcher.PitchAdjustmentRatio);
                    break;
            }

            return dataTable;
        }
    }
}