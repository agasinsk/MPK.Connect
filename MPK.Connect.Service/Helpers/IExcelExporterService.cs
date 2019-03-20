using System.Data;

namespace MPK.Connect.Service.Helpers
{
    public interface IExcelExporterService
    {
        void ExportToExcel(DataTable data, DataTable solutionDataTable, string filePath = null);
    }
}