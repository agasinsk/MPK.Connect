using MPK.Connect.Model.Helpers;

namespace MPK.Connect.DataAccess
{
    public class BaseRepository<T> : GenericRepository<T, string> where T : IdentifiableEntity<string>
    {
        public BaseRepository(IMpkContext context) : base(context)
        {
        }
    }
}