using MPK.Connect.Model;
using System.Linq;

namespace MPK.Connect.DataAccess.Agencies
{
    public class AgencyRepository : GenericRepository<MpkContext, Agency>, IAgencyRepository
    {
        public Agency GetSingle(string agencyId)
        {
            return GetAll().FirstOrDefault(a => a.Id == agencyId);
        }
    }
}