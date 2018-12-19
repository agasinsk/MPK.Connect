using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;

namespace MPK.Connect.Service.Business.Graph
{
    public interface IPathFinder
    {
        Path<T> FindShortestPath<TId, T>(Graph<TId, T> graph, T source, T destination)
            where TId : class
            where T : LocalizableEntity<TId>;
    }
}