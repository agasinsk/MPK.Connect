using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPK.Connect.Model
{
    public class Trip
    {
        public int BrigadeId { get; set; }

        [ForeignKey(nameof(ServiceId))]
        public Calendar Calendar { get; set; }

        [ForeignKey(nameof(ServiceId))]
        public CalendarDate CalendarDate { get; set; }

        public int DirectionId { get; set; }
        public string HeadSign { get; set; }

        [Key]
        public int Id { get; set; }

        public Route Route { get; set; }
        public int RouteId { get; set; }
        public int ServiceId { get; set; }
        public Shape Shape { get; set; }
        public int ShapeId { get; set; }
        public string TripId { get; set; }
        public Variant Variant { get; set; }
        public int VariantId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int VehicleId { get; set; }
    }
}