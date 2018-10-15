using MPK.Connect.Model.Helpers;

namespace MPK.Connect.DataAccess
{
    public class BaseRepository<T> : GenericRepository<MpkContext, T, string> where T : IdentifiableEntity<string>
    {
    }
}