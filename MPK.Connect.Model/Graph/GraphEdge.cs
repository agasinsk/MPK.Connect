using System;

namespace MPK.Connect.Model.Graph
{
    public class GraphEdge<TId>
    {
        public int Cost { get; set; }

        public TId DestinationId { get; set; }

        public TId SourceId { get; set; }

        public GraphEdge(TId sourceId, TId destinationId, int cost)
        {
            SourceId = sourceId;
            DestinationId = destinationId;
            Cost = cost;
        }

        public GraphEdge()
        {
        }
    }

    public class StopGraphEdge<TId> : GraphEdge<TId>
    {
        public string TripId { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public string RouteId { get; set; }

        public override string ToString()
        {
            return $"-> {DestinationId} | {RouteId} | {DepartureTime} | {Cost} min.";
        }
    }
}