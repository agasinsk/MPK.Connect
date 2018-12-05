using System.Collections.Generic;

namespace MPK.Connect.Model.Graph
{
    public class NodeCollection<T> : List<Node<T>>
    {
        public NodeCollection()
        {
        }

        public NodeCollection(int initialSize) : base(initialSize)
        {
        }

        public Node<T> FindByValue(T value)
        {
            return Find(n => n.Value.Equals(value));
        }
    }
}