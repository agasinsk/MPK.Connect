using System;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;

namespace MPK.Connect.Service.Business.Graph
{
    public interface IGraphBuilder
    {
        Graph<int, StopTimeInfo> GetGraph(DateTime startDate, CoordinateLimits graphLimits = null);

        Graph<int, StopDto> GetStopGraph(DateTime startDate);
    }
}