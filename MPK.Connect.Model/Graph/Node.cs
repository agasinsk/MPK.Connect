namespace MPK.Connect.Model.Graph
{
    public class Node<T>
    {
        public T Value { get; set; }

        protected NodeCollection<T> Neighbors { get; set; }

        public Node()
        {
        }

        public Node(T data) : this(data, null)
        {
        }

        public Node(T data, NodeCollection<T> neighbors)
        {
            Value = data;
            Neighbors = neighbors;
        }
    }
}