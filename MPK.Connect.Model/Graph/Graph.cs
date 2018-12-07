using MPK.Connect.Model.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MPK.Connect.Model.Graph
{
    public class Graph<TId, T> : IEnumerable<T> where T : IdentifiableEntity<TId>
    {
        public Dictionary<TId, GraphNode<TId, T>> Nodes { get; }

        public Graph() : this(null)
        {
        }

        public Graph(Dictionary<TId, GraphNode<TId, T>> nodeSet)
        {
            Nodes = nodeSet ?? new Dictionary<TId, GraphNode<TId, T>>();
        }

        public void AddDirectedEdge(T source, T destination, int edgeCost = 0)
        {
            var sourceNode = Nodes[source.Id];
            sourceNode?.Neighbors.Add(new GraphEdge<TId>(source.Id, destination.Id, edgeCost));
        }

        public void AddNode(GraphNode<TId, T> newNode)
        {
            Nodes[newNode.Id] = newNode;
        }

        public void AddNode(T value)
        {
            Nodes[value.Id] = new GraphNode<TId, T>(value);
        }

        public void AddNodes(IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                Nodes[value.Id] = new GraphNode<TId, T>(value);
            }
        }

        public void AddNodes(IEnumerable<GraphNode<TId, T>> nodes)
        {
            foreach (var node in nodes)
            {
                Nodes[node.Id] = node;
            }
        }

        public void AddUndirectedEdge(OldGraphNode<T> sourceNode, OldGraphNode<T> destinationNode, int edgeCost = 0)
        {
            sourceNode.Neighbors.Add(destinationNode);
            sourceNode.Costs.Add(edgeCost);

            destinationNode.Neighbors.Add(sourceNode);
            destinationNode.Costs.Add(edgeCost);
        }

        public bool Contains(T value)
        {
            return Nodes.ContainsKey(value.Id);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Nodes.Select(n => n.Value.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Remove(T value)
        {
            // first remove the node sourceNode the nodeset
            if (Nodes.ContainsKey(value.Id))
            {
                Nodes.Remove(value.Id);
            }

            // enumerate through each node in the nodeSet, removing edges to this node
            foreach (var node in Nodes)
            {
                var graphNode = node.Value;
                //graphNode.Neighbors.Remove();
            }

            return true;
        }
    }
}