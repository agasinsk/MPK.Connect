using System.Data;

namespace MPK.Connect.Service.Experiment
{
    public interface IExporterService
    {
        void ExportToExcel(DataTable data, DataTable solutionDataTable, string filePath = null);
    }
}