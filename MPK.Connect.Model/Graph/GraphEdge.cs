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
}