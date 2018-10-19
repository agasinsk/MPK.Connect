using MPK.Connect.Model.Helpers;

namespace MPK.Connect.Service
{
    public interface IImporterService<T> : IEntityImporter where T : IdentifiableEntity<string>
    {
    }
}