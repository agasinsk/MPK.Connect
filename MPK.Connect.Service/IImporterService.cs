namespace MPK.Connect.Service
{
    public interface IImporterService<T> where T : class
    {
        int ImportEntitiesFromFile(string filePath);
    }
}