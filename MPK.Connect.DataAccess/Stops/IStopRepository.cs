using MPK.Connect.Model;

namespace MPK.Connect.DataAccess.Stops
{
    public interface IStopRepository : IGenericRepository<Stop>
    {
        Stop GetSingle(int stopId);
    }
}