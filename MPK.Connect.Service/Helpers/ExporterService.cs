using System;
using System.Data;
using System.IO;
using OfficeOpenXml;

namespace MPK.Connect.Service.Experiment
{
    public class ExporterService : IExporterService
    {
        private const string DefaultFileName = "../../../results/HarmonySearchTestResults";
        private const string DefaultPath = "../../../results/";
        private const string HarmonySearchSheetName = "Harmony search";

        public ExporterService()
        {
            Directory.CreateDirectory(DefaultPath);
        }

        public void ExportToExcel(DataTable data, DataTable solutionDataTable, string filePath)
        {
            using (var excelPackage = new ExcelPackage())
            {
                var excelWorksheet = excelPackage.Workbook.Worksheets.Add(HarmonySearchSheetName);

                var range = excelWorksheet.Cells.LoadFromDataTable(data, false);

                excelWorksheet.Cells[1, range.End.Column + 2].LoadFromDataTable(solutionDataTable, true);

                excelWorksheet.Cells.AutoFitColumns();

                if (!string.IsNullOrEmpty(filePath))
                {
                    var providedPath = Path.GetDirectoryName(filePath);
                    Directory.CreateDirectory(Path.Combine(DefaultPath, providedPath));
                }

                var excelFilename = string.IsNullOrEmpty(filePath) ? DefaultFileName : Path.Combine(DefaultPath, filePath);
                excelPackage.SaveAs(new FileInfo($"{excelFilename}_{DateTime.Now:ddMMyyyy_HHmmsss}.xlsx"));
            }
        }
    }
}