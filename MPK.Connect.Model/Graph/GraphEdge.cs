using System;

namespace MPK.Connect.Model.Graph
{
    public class GraphEdge<TId>
    {
        public double Cost { get; set; }

        public TId DestinationId { get; set; }

        public TId SourceId { get; set; }

        public GraphEdge(TId sourceId, TId destinationId, double cost)
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
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public string RouteId { get; set; }
        public string TripId { get; set; }

        public override string ToString()
        {
            return $"-> {DestinationId} | {RouteId} | {DepartureTime} | {Cost} min.";
        }
    }
}