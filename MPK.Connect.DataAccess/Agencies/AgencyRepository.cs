using System.Linq;
using MPK.Connect.Model;

namespace MPK.Connect.DataAccess.Agencies
{
    public class AgencyRepository : GenericRepository<MpkContext, Agency>, IAgencyRepository
    {
        public Agency GetSingle(int agencyId)
        {
            return GetAll().FirstOrDefault(a => a.AgencyId == agencyId);
        }
    }
}