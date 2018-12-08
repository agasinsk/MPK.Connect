using System.Collections.Generic;

namespace MPK.Connect.Model.Graph
{
    public class OldGraphNode<T> : Node<T>
    {
        private List<int> costs;

        public List<int> Costs => costs ?? (costs = new List<int>());
        public new NodeCollection<T> Neighbors => base.Neighbors ?? (base.Neighbors = new NodeCollection<T>());

        public OldGraphNode()
        {
        }

        public OldGraphNode(T value) : base(value)
        {
        }

        public OldGraphNode(T value, NodeCollection<T> neighbors) : base(value, neighbors)
        {
        }
    }
}