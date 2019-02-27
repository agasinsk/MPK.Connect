using MPK.Connect.Model.Helpers;

namespace MPK.Connect.DataAccess
{
    public class BaseRepository<T> : GenericRepository<T> where T : class, IIdentifiable
    {
        public BaseRepository(IMpkContext context) : base(context)
        {
        }
    }
}