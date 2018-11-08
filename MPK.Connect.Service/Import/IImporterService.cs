using MPK.Connect.Model.Helpers;

namespace MPK.Connect.Service.Import
{
    public interface IImporterService<T> : IEntityImporter where T : IdentifiableEntity<string>
    {
    }
}