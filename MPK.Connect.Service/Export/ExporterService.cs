using System.IO;
using OfficeOpenXml;

namespace MPK.Connect.Service.Export
{
    public class ExporterService
    {
        public void ExportToExcel(string fileName)
        {
            using (var excelPackage = new ExcelPackage())
            {
                //A workbook must have at least on cell, so lets add one...
                var excelWorksheet = excelPackage.Workbook.Worksheets.Add("MySheet");
                //To set values in the spreadsheet use the Cells indexer.
                excelWorksheet.Cells["A1"].Value = "This is cell A1";
                //Save the new workbook. We haven't specified the filename so use the Save as method.
                excelPackage.SaveAs(new FileInfo(fileName));
            }
        }
    }
}