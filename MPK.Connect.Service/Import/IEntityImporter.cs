namespace MPK.Connect.Service.Import
{
    public interface IEntityImporter
    {
        int ImportEntitiesFromFile(string filePath);
    }
}