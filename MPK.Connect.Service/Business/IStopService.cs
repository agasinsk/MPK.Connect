using System.Collections.Generic;
using MPK.Connect.Model.Business;

namespace MPK.Connect.Service.Business
{
    public interface IStopService
    {
        List<StopDto> GetAllStops();

        List<StopDto> GetStopByName(string stopName);

        StopDto GetStopById(string stopId);

        List<StopDto> GetDistinctStopsByName();
    }
}