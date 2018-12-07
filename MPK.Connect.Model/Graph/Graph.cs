﻿using MPK.Connect.Model.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MPK.Connect.Model.Graph
{
    public class Graph<TId, T> : IEnumerable<T> where T : IdentifiableEntity<TId> where TId : class
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

        public IEnumerable<GraphNode<TId, T>> LetDijkstraFindShortestPath(TId source, TId destination)
        {
            // Initialize distance and route tables
            var distances = new Dictionary<TId, int>();
            var routes = new Dictionary<TId, TId>();

            foreach (var stopId in Nodes.Keys)
            {
                distances[stopId] = int.MaxValue;
            }

            distances[source] = 0;

            var stopIds = new List<TId>(Nodes.Keys);	// nodes == Q

            /**** START DIJKSTRA ****/
            while (stopIds.Count > 0)
            {
                // get the minimum node
                var minDist = int.MaxValue;
                TId minimumStopId = null;
                foreach (var stopId in stopIds)
                {
                    if (distances[stopId] <= minDist)
                    {
                        minDist = distances[stopId];
                        minimumStopId = stopId;
                    }
                }

                // remove it from the set Q
                stopIds.Remove(minimumStopId);

                // iterate through all of u's neighbors
                var minimumNode = Nodes[minimumStopId];
                if (minimumNode.Neighbors != null)
                {
                    // relax each edge
                    foreach (var neighbor in minimumNode.Neighbors)
                    {
                        var distTouCity = distances[minimumStopId];
                        var distTovCity = distances[neighbor.DestinationId];

                        if (distTovCity > distTouCity + neighbor.Cost)
                        {
                            // update distance and route
                            distances[neighbor.DestinationId] = distTouCity + neighbor.Cost;
                            routes[neighbor.DestinationId] = minimumNode.Id;
                        }
                    }
                }
            }
            /**** END DIJKSTRA ****/

            // Track the path
            var traceBackSteps = new List<GraphNode<TId, T>>();
            var destinationNode = Nodes[destination];
            traceBackSteps.Add(destinationNode);
            var currentNodeId = destinationNode.Id;
            do
            {
                currentNodeId = routes[currentNodeId];
                var currentNode = Nodes[currentNodeId];
                traceBackSteps.Add(currentNode);
            } while (currentNodeId != source);

            traceBackSteps.Reverse();
            return traceBackSteps;
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