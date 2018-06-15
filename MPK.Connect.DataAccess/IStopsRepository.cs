using MPK.Connect.Model;
using System.Collections.Generic;

namespace MPK.Connect.DataAccess
{
    public interface IStopsRepository
    {
        Stop CreateStop(Stop stop);

        List<Stop> CreateStops(List<Stop> stops);

        Stop DeleteStop(Stop stop);

        Stop GetStop(int stopId);

        Stop UpdateStop(Stop stop);
    }
}