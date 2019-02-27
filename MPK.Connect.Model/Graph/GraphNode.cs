using System.Collections.Generic;
using MPK.Connect.Model.Helpers;

namespace MPK.Connect.Model.Graph
{
    public class GraphNode<TId, T> : Identifiable<TId> where T : Identifiable<TId>
    {
        public ICollection<GraphEdge<TId>> Neighbors { get; set; }
        public T Data { get; set; }
        public override TId Id => Data.Id;

        public GraphNode()
        {
        }

        public GraphNode(T data)
        {
            Data = data;
            Neighbors = new List<GraphEdge<TId>>();
        }

        public GraphNode(T data, ICollection<GraphEdge<TId>> neighbors)
        {
            Data = data;
            Neighbors = neighbors;
        }

        public override string ToString()
        {
            return $"{Data} : Neighbors {Neighbors.Count}";
        }
    }
}