using MPK.Connect.Model;
using System.Linq;

namespace MPK.Connect.DataAccess.Stops
{
    public class StopRepository : GenericRepository<MpkContext, Stop>, IStopRepository
    {
        public Stop GetSingle(string stopId)
        {
            var query = GetAll().FirstOrDefault(s => s.Id == stopId);
            return query;
        }
    }
}