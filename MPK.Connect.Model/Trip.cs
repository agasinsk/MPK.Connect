namespace MPK.Connect.Model
{
    public class Trip
    {
        public int BrigadeId { get; set; }
        public int DirectionId { get; set; }
        public string HeadSign { get; set; }

        public string Id { get; set; }

        public Route Route { get; set; }
        public int RouteId { get; set; }
        public int ServiceId { get; set; }
        public Shape Shape { get; set; }
        public int ShapeId { get; set; }
        public Variant Variant { get; set; }
        public int VariantId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int VehicleId { get; set; }
    }
}