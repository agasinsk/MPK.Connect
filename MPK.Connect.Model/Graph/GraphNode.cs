using MPK.Connect.Model.Helpers;
using System.Collections.Generic;

namespace MPK.Connect.Model.Graph
{
    public class GraphNode<TId, T> : IdentifiableEntity<TId> where T : IdentifiableEntity<TId>
    {
        public override TId Id => Value.Id;
        public ICollection<GraphEdge<TId>> Neighbors { get; set; }
        public T Value { get; set; }

        public GraphNode()
        {
        }

        public GraphNode(T value)
        {
            Value = value;
            Neighbors = new List<GraphEdge<TId>>();
        }

        public GraphNode(T value, ICollection<GraphEdge<TId>> neighbors)
        {
            Value = value;
            Neighbors = neighbors;
        }
    }
}