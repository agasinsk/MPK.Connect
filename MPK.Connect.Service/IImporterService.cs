using MPK.Connect.Model.Helpers;

namespace MPK.Connect.Service
{
    public interface IImporterService<T> where T : IdentifiableEntity<string>
    {
        int ImportEntitiesFromFile(string filePath);
    }
}