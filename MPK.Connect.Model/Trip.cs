using MPK.Connect.Model.Enums;
using MPK.Connect.Model.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPK.Connect.Model
{
    public class Trip : IdentifiableEntity<string>
    {
        [Required]
        public override string Id { get; set; }

        [Required]
        public string RouteId { get; set; }

        [Required]
        public string ServiceId { get; set; }

        public string HeadSign { get; set; }
        public string ShortName { get; set; }

        public int? DirectionId { get; set; }
        public string BlockId { get; set; }
        public string ShapeId { get; set; }

        public WheelchairBoardings WheelchairAccessible { get; set; }
        public BikesAllowed BikesAllowed { get; set; }

        public Route Route { get; set; }

        [ForeignKey("ShapeId")]
        public ShapeBase Shape { get; set; }

        [ForeignKey("ServiceId")]
        public Calendar Calendar { get; set; }
    }
}