using System;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;

namespace MPK.Connect.Service.Business.Graph
{
    public interface IGraphBuilder
    {
        Graph<string, StopTimeInfo> GetGraph(DateTime startDate, CoordinateLimits graphLimits = null);
    }
}