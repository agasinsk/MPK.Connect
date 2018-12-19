using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;

namespace MPK.Connect.Service.Business.Graph
{
    public interface IStopPathFinder
    {
        Path<StopTimeInfo> FindShortestPath(Graph<string, StopTimeInfo> graph, StopTimeInfo source,
            string destinationName);
    }
}