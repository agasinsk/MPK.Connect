using System.Data;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.ParameterProviders;

namespace MPK.Connect.Service.Helpers
{
    public static class HarmonySearcherExtensions
    {
        public static DataTable ToDataTable<T>(this IHarmonySearcher<T> harmonySearcher)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Property", typeof(string));
            dataTable.Columns.Add("Value", typeof(string));

            dataTable.Rows.Add(nameof(harmonySearcher.Type), harmonySearcher.Type.ToString());
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
    }
}