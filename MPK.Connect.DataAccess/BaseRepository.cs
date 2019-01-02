using MPK.Connect.Model.Helpers;

namespace MPK.Connect.DataAccess
{
    public class BaseRepository<T> : GenericRepository<T> where T : class, IIdentifiableEntity
    {
        public BaseRepository(IMpkContext context) : base(context)
        {
        }
    }
}