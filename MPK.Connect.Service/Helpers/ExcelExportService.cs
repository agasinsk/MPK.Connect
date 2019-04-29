using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;

namespace MPK.Connect.Service.Helpers
{
    public class ExcelExportService : IExcelExportService
    {
        private const string DefaultFileName = "../../../results/HarmonySearchTestResults";
        private const string DefaultPath = "../../../results/";
        private const string HarmonySearchSheetName = "Harmony search";

        public ExcelExportService() => Directory.CreateDirectory(DefaultPath);

        public void ExportToExcel(DataTable infoDataTable, DataTable parameterDataTable, DataTable solutionsDataTable, string filePath = null)
        {
            using (var excelPackage = new ExcelPackage())
            {
                var excelWorksheet = excelPackage.Workbook.Worksheets.Add(HarmonySearchSheetName);

                var range = excelWorksheet.Cells.LoadFromDataTable(infoDataTable, false, TableStyles.Light1);

                if (parameterDataTable != null)
                {
                    range = excelWorksheet.Cells[range.End.Row + 2, 1].LoadFromDataTable(parameterDataTable, false);
                }

                excelWorksheet.Cells[1, range.End.Column + 2].LoadFromDataTable(solutionsDataTable, true, TableStyles.Light1);

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
                CreateSheetWithDataTables(excelPackage, HarmonySearchSheetName, infoDataTable, dataTables);

                SaveFile(filePath, excelPackage);
            }
        }

        public void ExportToExcel(DataTable infoDataTable, Dictionary<string, List<DataTable>> dataTableForSheets, string filePath)
        {
            using (var excelPackage = new ExcelPackage())
            {
                foreach (var (worksheetName, dataTables) in dataTableForSheets)
                {
                    CreateSheetWithDataTables(excelPackage, worksheetName, infoDataTable, dataTables);
                }

                SaveFile(filePath, excelPackage);
            }
        }

        private static void SaveFile(string filePath, ExcelPackage excelPackage)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                var providedPath = Path.GetDirectoryName(filePath);
                Directory.CreateDirectory(Path.Combine(DefaultPath, providedPath));
            }

            var excelFilename = string.IsNullOrEmpty(filePath) ?
                DefaultFileName :
                Path.Combine(DefaultPath, filePath);

            excelPackage.SaveAs(new FileInfo($"{excelFilename}_{DateTime.Now:ddMMyyyy_HHmmsss}.xlsx"));
        }

        private void CreateSheetWithDataTables(ExcelPackage excelPackage, string worksheetName, DataTable infoDataTable, List<DataTable> dataTables)
        {
            var excelWorksheet = excelPackage.Workbook.Worksheets.Add(worksheetName);

            var range = excelWorksheet.Cells.LoadFromDataTable(infoDataTable, false, TableStyles.Light18);

            foreach (var dataTable in dataTables)
            {
                range = WriteTableName(excelWorksheet, range, dataTable);

                range = excelWorksheet.Cells[range.End.Row + 1, 1].LoadFromDataTable(dataTable, true, TableStyles.Light15);

                foreach (var cell in excelWorksheet.Cells[range.Start.Row + 1, 1, range.End.Row, range.End.Column])
                {
                    var cellContent = cell.Value.ToString();
                    if (double.TryParse(cellContent, out var cellValue) && !int.TryParse(cellContent, out var cellIntValue))
                    {
                        cell.Style.Numberformat.Format = "0.000";
                    }

                    if (cellContent.Contains('/'))
                    {
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Fill.BackgroundColor.SetColor(Color.White);
                }
            }

            excelWorksheet.Cells.AutoFitColumns();
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