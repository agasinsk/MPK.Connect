using System.Collections.Generic;
using System.Data;

namespace MPK.Connect.Service.Helpers
{
    public interface IExcelExporterService
    {
        void ExportToExcel(DataTable infoDataTable, DataTable parameterDataTable, DataTable solutionDataTable, string filePath = null);

        void ExportToExcel(List<DataTable> dataTables, string filePath = null);
    }
}