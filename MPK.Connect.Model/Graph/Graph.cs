using MPK.Connect.Model.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MPK.Connect.Model.Graph
{
    public class Graph<TId, T> : IEnumerable<T> where T : IdentifiableEntity<TId> where TId : class
    {
        public Dictionary<TId, GraphNode<TId, T>>.KeyCollection Keys => Nodes.Keys;
        public Dictionary<TId, GraphNode<TId, T>> Nodes { get; }

        public GraphNode<TId, T> this[TId key]
        {
            get => Nodes[key];
            set => Nodes[key] = value;
        }

        public Graph()
        {
        }

        public Graph(Dictionary<TId, GraphNode<TId, T>> nodeSet)
        {
            Nodes = nodeSet ?? new Dictionary<TId, GraphNode<TId, T>>();
        }

        public Graph(Dictionary<TId, T> nodeSet)
        {
            var nodes = nodeSet.ToDictionary(k => k.Key, v => new GraphNode<TId, T>(v.Value));
            Nodes = nodes;
        }

        public void AddDirectedEdge(T source, T destination, double edgeCost = 0)
        {
            var sourceNode = Nodes[source.Id];
            sourceNode?.Neighbors.Add(new GraphEdge<TId>(source.Id, destination.Id, edgeCost));
        }

        public void AddEdge(T source, T destination, double edgeCost = 0)
        {
            var sourceNode = Nodes[source.Id];
            sourceNode?.Neighbors.Add(new GraphEdge<TId>(source.Id, destination.Id, edgeCost));

            var destinationNode = Nodes[destination.Id];
            destinationNode?.Neighbors.Add(new GraphEdge<TId>(destination.Id, source.Id, edgeCost));
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

        public bool Contains(T value)
        {
            return Nodes.ContainsKey(value.Id);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Nodes.Select(n => n.Value.Data).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Remove(T valueToRemove)
        {
            // first remove the node sourceNode the nodeset
            if (Nodes.ContainsKey(valueToRemove.Id))
            {
                Nodes.Remove(valueToRemove.Id);
            }

            // enumerate through each node in the nodeSet, removing edges to this node
            foreach (var node in Nodes)
            {
                var graphNode = node.Value;
                var neighbor = graphNode.Neighbors.FirstOrDefault(n => n.DestinationId.Equals(valueToRemove.Id));
                if (neighbor != null)
                {
                    graphNode.Neighbors.Remove(neighbor);
                }
            }

            return true;
        }
    }
}