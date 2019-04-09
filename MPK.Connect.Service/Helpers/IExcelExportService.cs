using System.Collections.Generic;
using System.Data;

namespace MPK.Connect.Service.Helpers
{
    public interface IExcelExportService
    {
        void ExportToExcel(DataTable infoDataTable, DataTable parameterDataTable, DataTable solutionsDataTable, string filePath = null);

        void ExportToExcel(DataTable infoDataTable, List<DataTable> dataTables, string filePath = null);

        void ExportToExcel(DataTable infoDataTable, Dictionary<string, List<DataTable>> dataTableForSheets, string filePath);
    }
}