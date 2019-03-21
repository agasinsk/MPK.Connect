using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;

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

                var range = excelWorksheet.Cells.LoadFromDataTable(infoDataTable, false, TableStyles.Light1);

                range = excelWorksheet.Cells[range.End.Row + 2, 1].LoadFromDataTable(parameterDataTable, false);

                excelWorksheet.Cells[range.Start.Row, range.End.Column + 2].LoadFromDataTable(solutionDataTable, true, TableStyles.Light1);

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

        public void ExportToExcel(DataTable infoDataTable, List<DataTable> dataTables, string filePath = null)
        {
            using (var excelPackage = new ExcelPackage())
            {
                var excelWorksheet = excelPackage.Workbook.Worksheets.Add(HarmonySearchSheetName);

                var range = excelWorksheet.Cells.LoadFromDataTable(infoDataTable, false, TableStyles.Light18);

                foreach (var dataTable in dataTables)
                {
                    range = WriteTableName(excelWorksheet, range, dataTable);

                    range = excelWorksheet.Cells[range.End.Row + 1, 1].LoadFromDataTable(dataTable, true, TableStyles.Light1);
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

        private ExcelRangeBase WriteTableName(ExcelWorksheet excelWorksheet, ExcelRangeBase range, DataTable dataTable)
        {
            var row = range.End.Row + 2;

            range = excelWorksheet.Cells[row, 1, row, dataTable.Columns.Count].LoadFromText(dataTable.TableName);
            excelWorksheet.Cells[row, 1, row, dataTable.Columns.Count].Merge = true;
            excelWorksheet.Cells[range.Address].Style.Font.Bold = true;
            excelWorksheet.Cells[range.Address].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            excelWorksheet.Cells[range.Address].Style.Font.Size = 12;

            return range;
        }
    }
}