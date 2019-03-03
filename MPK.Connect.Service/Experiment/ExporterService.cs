using System;
using System.Data;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace MPK.Connect.Service.Experiment
{
    public class ExporterService : IExporterService
    {
        private const string DefaultFileName = "../../../results/HarmonySearchTestResults";

        private const string HarmonySearchSheetName = "Harmony search";

        public ExporterService()
        {
            Directory.CreateDirectory("../../../results/");
        }

        public void ExportToExcel(DataTable data, DataTable solutionDataTable, string fileName)
        {
            using (var excelPackage = new ExcelPackage())
            {
                var excelWorksheet = excelPackage.Workbook.Worksheets.Add(HarmonySearchSheetName);

                var range = excelWorksheet.Cells.LoadFromDataTable(data, false, TableStyles.Light1);

                excelWorksheet.Cells[1, range.End.Column + 2].LoadFromDataTable(solutionDataTable, true, TableStyles.Light1);

                excelWorksheet.Cells.AutoFitColumns();

                var excelFilename = string.IsNullOrEmpty(fileName) ? DefaultFileName : fileName;
                excelPackage.SaveAs(new FileInfo($"{excelFilename}_{DateTime.Now:ddMMyyyy_HHmmsss}.xlsx"));
            }
        }
    }
}