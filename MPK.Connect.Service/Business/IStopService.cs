using MPK.Connect.Model.Business;
using System.Collections.Generic;

namespace MPK.Connect.Service.Business
{
    public interface IStopService
    {
        List<StopDto> GetAllStops();

        List<StopDto> GetStopByName(string stopName);

        StopDto GetStopById(int stopId);

        List<StopDto> GetDistinctStopsByName();
    }
}