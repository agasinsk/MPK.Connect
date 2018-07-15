using MPK.Connect.Model;

namespace MPK.Connect.DataAccess.Agencies
{
    public interface IAgencyRepository : IGenericRepository<Agency>
    {
        Agency GetSingle(int agencyId);
    }
}