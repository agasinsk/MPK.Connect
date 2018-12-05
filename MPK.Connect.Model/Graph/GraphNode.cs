using System.Collections.Generic;

namespace MPK.Connect.Model.Graph
{
    public class GraphNode<T> : Node<T>
    {
        private List<int> costs;

        public List<int> Costs => costs ?? (costs = new List<int>());
        public new NodeCollection<T> Neighbors => base.Neighbors ?? (base.Neighbors = new NodeCollection<T>());

        public GraphNode()
        {
        }

        public GraphNode(T value) : base(value)
        {
        }

        public GraphNode(T value, NodeCollection<T> neighbors) : base(value, neighbors)
        {
        }
    }
}