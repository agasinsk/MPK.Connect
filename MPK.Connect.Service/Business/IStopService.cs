using MPK.Connect.Model;
using System.Collections.Generic;

namespace MPK.Connect.Service.Business
{
    public interface IStopService
    {
        List<Stop> GetAllStops();

        List<Stop> GetStopByName(string stopName);

        Stop GetStopById(string stopId);
    }
}