using System.Data;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace MPK.Connect.Service.Experiment
{
    public class ExporterService : IExporterService
    {
        private const string DefaultFileName = "../../../HarmonySearchTestResults.xlsx";
        private const string HarmonySearchSheetName = "Harmony search";

        public void ExportToExcel(DataTable data, DataTable solutionDataTable, string fileName)
        {
            using (var excelPackage = new ExcelPackage())
            {
                var excelWorksheet = excelPackage.Workbook.Worksheets.Add(HarmonySearchSheetName);

                var range = excelWorksheet.Cells.LoadFromDataTable(data, false, TableStyles.Light1);

                excelWorksheet.Cells[1, range.End.Column + 2].LoadFromDataTable(solutionDataTable, true, TableStyles.Light1);

                excelWorksheet.Cells.AutoFitColumns();

                //Save the new workbook. We haven't specified the filename so use the Save as method.
                excelPackage.SaveAs(new FileInfo(string.IsNullOrEmpty(fileName) ? DefaultFileName : fileName));
            }
        }
    }
}