using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using OfficeOpenXml;

namespace MPK.Connect.Service.Helpers
{
    public class ExcelExporterService : IExcelExporterService
    {
        private const string DefaultFileName = "../../../results/HarmonySearchTestResults";
        private const string DefaultPath = "../../../results/";
        private const string HarmonySearchSheetName = "Harmony search";

        public ExcelExporterService()
        {
            Directory.CreateDirectory(DefaultPath);
        }

        public void ExportToExcel(DataTable infoDataTable, DataTable parameterDataTable, DataTable solutionDataTable,
            string filePath = null)
        {
            using (var excelPackage = new ExcelPackage())
            {
                var excelWorksheet = excelPackage.Workbook.Worksheets.Add(HarmonySearchSheetName);

                var range = excelWorksheet.Cells.LoadFromDataTable(infoDataTable, false);

                range = excelWorksheet.Cells[range.End.Row + 2, 1].LoadFromDataTable(parameterDataTable, false);

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

        public void ExportToExcel(List<DataTable> dataTables, string filePath = null)
        {
            using (var excelPackage = new ExcelPackage())
            {
                var excelWorksheet = excelPackage.Workbook.Worksheets.Add(HarmonySearchSheetName);

                var range = new ExcelAddress(1, 1, 1, 1);
                foreach (var dataTable in dataTables)
                {
                    excelWorksheet.Cells[range.End.Row + 2, 1].LoadFromText(dataTable.TableName);

                    range = excelWorksheet.Cells[range.End.Row + 3, 1].LoadFromDataTable(dataTable, true);
                }

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