using System.Collections.Generic;

namespace MPK.Connect.Service.Business.Graph
{
    public interface IStopMapManager
    {
        IEnumerable<string> InitializeGraph();
    }
}