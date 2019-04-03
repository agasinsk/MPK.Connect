using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model.Helpers;

namespace MPK.Connect.Model.Graph
{
    public class Graph<TId, T> : IEnumerable<T> where T : Identifiable<TId>
    {
        private Dictionary<TId, Dictionary<TId, double>> _edges;

        public Dictionary<TId, ICollection<GraphEdge<TId>>> Edges =>
         Nodes.Values.ToDictionary(n => n.Id, v => v.Neighbors);

        public Dictionary<TId, GraphNode<TId, T>> Nodes { get; }
        public Dictionary<TId, GraphNode<TId, T>>.KeyCollection Keys => Nodes.Keys;

        public Graph()
        {
            Nodes = new Dictionary<TId, GraphNode<TId, T>>();
        }

        public Graph(Dictionary<TId, GraphNode<TId, T>> nodeSet)
        {
            Nodes = nodeSet ?? new Dictionary<TId, GraphNode<TId, T>>();
        }

        public Graph(Dictionary<TId, T> nodeSet)
        {
            Nodes = nodeSet.ToDictionary(k => k.Key, v => new GraphNode<TId, T>(v.Value));
        }

        public GraphNode<TId, T> this[TId key]
        {
            get => Nodes[key];
            set => Nodes[key] = value;
        }

        public void AddDirectedEdge(T source, T destination, double edgeCost = 0)
        {
            var sourceNode = Nodes[source.Id];
            if (sourceNode != null && sourceNode.Neighbors.All(n => !n.DestinationId.Equals(destination.Id)))
            {
                sourceNode.Neighbors.Add(new GraphEdge<TId>(destination.Id, edgeCost));
            }
        }

        public void AddEdge(T source, T destination, double edgeCost = 0)
        {
            var sourceNode = Nodes[source.Id];
            sourceNode?.Neighbors.Add(new GraphEdge<TId>(destination.Id, edgeCost));

            var destinationNode = Nodes[destination.Id];
            destinationNode?.Neighbors.Add(new GraphEdge<TId>(source.Id, edgeCost));
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

        public Dictionary<TId, Dictionary<TId, double>> GetEdges()
        {
            return _edges ?? (_edges = Nodes.Values
                       .ToDictionary(n => n.Id, v => v.Neighbors
                           .ToDictionary(x => x.DestinationId, x => x.Cost)));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Nodes.Select(n => n.Value.Data).GetEnumerator();
        }

        public IEnumerable<GraphNode<TId, T>> GetNeighbors(TId sourceId)
        {
            return Nodes[sourceId].Neighbors.Select(n => Nodes[n.DestinationId]).ToList();
        }

        public IQueryable<GraphNode<TId, T>> GetNeighborsQueryable(TId sourceId)
        {
            return Nodes[sourceId].Neighbors.Select(n => Nodes[n.DestinationId]).AsQueryable();
        }

        public bool Remove(T valueToRemove)
        {
            // Remove the node from the node set
            if (Nodes.ContainsKey(valueToRemove.Id))
            {
                Nodes.Remove(valueToRemove.Id);
            }

            // Removing edges to this node
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}