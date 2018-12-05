using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MPK.Connect.Model.Graph
{
    public class Graph<T> : IEnumerable<T>
    {
        public NodeCollection<T> Nodes { get; }

        public int Count => Nodes.Count;

        public Graph() : this(null)
        {
        }

        public Graph(NodeCollection<T> nodeSet)
        {
            Nodes = nodeSet ?? new NodeCollection<T>();
        }

        public void AddNodes(IEnumerable<T> nodes)
        {
            var graphNodes = nodes.Select(v => new GraphNode<T>(v));
            Nodes.AddRange(graphNodes);
        }

        public void AddNodes(IEnumerable<GraphNode<T>> nodes)
        {
            Nodes.AddRange(nodes);
        }

        public void AddNode(GraphNode<T> node)
        {
            Nodes.Add(node);
        }

        public void AddNode(T value)
        {
            Nodes.Add(new GraphNode<T>(value));
        }

        public void AddDirectedEdge(T source, T destination, int edgeCost = 0)
        {
            var sourceNode = Nodes.FindByValue(source) as GraphNode<T>;
            sourceNode?.Neighbors.Add(Nodes.FindByValue(destination));
            sourceNode?.Costs.Add(edgeCost);
        }

        public void AddDirectedEdge(GraphNode<T> sourceNode, GraphNode<T> destinationNode, int edgeCost = 0)
        {
            sourceNode.Neighbors.Add(destinationNode);
            sourceNode.Costs.Add(edgeCost);
        }

        public void AddUndirectedEdge(GraphNode<T> sourceNode, GraphNode<T> destinationNode, int edgeCost = 0)
        {
            sourceNode.Neighbors.Add(destinationNode);
            sourceNode.Costs.Add(edgeCost);

            destinationNode.Neighbors.Add(sourceNode);
            destinationNode.Costs.Add(edgeCost);
        }

        public bool Contains(T value)
        {
            return Nodes.FindByValue(value) != null;
        }

        public bool Remove(T value)
        {
            // first remove the node sourceNode the nodeset
            var nodeToRemove = Nodes.FindByValue(value);
            if (nodeToRemove == null)
            {
                return false;
            }
            Nodes.Remove(nodeToRemove);

            // enumerate through each node in the nodeSet, removing edges to this node
            foreach (var node in Nodes)
            {
                var graphNode = node as GraphNode<T>;
                var index = graphNode.Neighbors.IndexOf(nodeToRemove);
                if (index != -1)
                {
                    // remove the reference to the node and associated cost
                    graphNode.Neighbors.RemoveAt(index);
                    graphNode.Costs.RemoveAt(index);
                }
            }

            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Nodes.Select(n => n.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}