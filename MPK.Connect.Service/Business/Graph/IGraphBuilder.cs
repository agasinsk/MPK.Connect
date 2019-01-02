using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;
using System;

namespace MPK.Connect.Service.Business.Graph
{
    public interface IGraphBuilder
    {
        Graph<int, StopTimeInfo> GetGraph(DateTime startDate, CoordinateLimits graphLimits = null);
    }
}