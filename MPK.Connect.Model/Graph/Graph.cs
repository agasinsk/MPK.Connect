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

        //public IEnumerable<GraphNode<TId, T>> LetDijkstraFindShortestPath(TId source, TId destination)
        //{
        //    // Initialize distance and route tables
        //    var distances = new Dictionary<TId, int>();
        //    var routes = new Dictionary<TId, TId>();

        //    foreach (var nodeId in Nodes.Keys)
        //    {
        //        distances[nodeId] = int.MaxValue;
        //    }

        //    distances[source] = 0;

        //    // nodes == Q
        //    var nodesIds = new List<TId>(Nodes.Keys);

        //    /**** START DIJKSTRA ****/
        //    while (nodesIds.Count > 0)
        //    {
        //        // get the minimum node
        //        var minimumDistance = int.MaxValue;
        //        TId minimumNodeId = null;
        //        foreach (var nodeId in nodesIds)
        //        {
        //            if (distances[nodeId] <= minimumDistance)
        //            {
        //                minimumDistance = distances[nodeId];
        //                minimumNodeId = nodeId;
        //            }
        //        }

        //        // remove it from the set Q
        //        nodesIds.Remove(minimumNodeId);

        //        // iterate through all of u's neighbors
        //        var minimumNode = Nodes[minimumNodeId];
        //        if (minimumNode.Neighbors != null)
        //        {
        //            // relax each edge
        //            foreach (var neighbor in minimumNode.Neighbors)
        //            {
        //                var distTouCity = distances[minimumNodeId];
        //                var distTovCity = distances[neighbor.DestinationId];

        //                if (distTovCity > distTouCity + neighbor.Cost)
        //                {
        //                    // update distance and route
        //                    distances[neighbor.DestinationId] = distTouCity + neighbor.Cost;
        //                    routes[neighbor.DestinationId] = minimumNode.Id;
        //                }
        //            }
        //        }
        //    }
        //    /**** END DIJKSTRA ****/

        //    // Track the path
        //    var traceBackSteps = new List<GraphNode<TId, T>>();
        //    var destinationNode = Nodes[destination];
        //    traceBackSteps.Add(destinationNode);
        //    var currentNodeId = destinationNode.Id;
        //    do
        //    {
        //        currentNodeId = routes[currentNodeId];
        //        var currentNode = Nodes[currentNodeId];
        //        traceBackSteps.Add(currentNode);
        //    } while (currentNodeId != source);

        //    traceBackSteps.Reverse();
        //    return traceBackSteps;
        //}

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