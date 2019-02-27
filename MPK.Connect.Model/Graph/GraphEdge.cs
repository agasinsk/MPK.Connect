namespace MPK.Connect.Model.Graph
{
    public class GraphEdge<TId>
    {
        public double Cost { get; set; }

        public TId DestinationId { get; set; }

        public GraphEdge(TId destinationId, double cost)
        {
            DestinationId = destinationId;
            Cost = cost;
        }

        public override string ToString()
        {
            return $"{DestinationId}, Cost:{Cost}";
        }
    }
}