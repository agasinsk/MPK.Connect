using System.Linq;
using MPK.Connect.Model;

namespace MPK.Connect.DataAccess.Stops
{
    public class StopRepository : GenericRepository<MpkContext, Stop>, IStopRepository
    {
        public Stop GetSingle(int stopId)
        {
            var query = GetAll().FirstOrDefault(s => s.Id == stopId);
            return query;
        }
    }
}