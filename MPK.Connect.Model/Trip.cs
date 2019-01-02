using MPK.Connect.Model.Enums;
using MPK.Connect.Model.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPK.Connect.Model
{
    public class Trip : IdentifiableEntity<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public override int Id { get; set; }

        [Required]
        public string RouteId { get; set; }

        public int ServiceId { get; set; }

        public string HeadSign { get; set; }
        public string ShortName { get; set; }

        public int? DirectionId { get; set; }
        public string BlockId { get; set; }
        public string ShapeId { get; set; }

        public WheelchairBoardings WheelchairAccessible { get; set; }
        public BikesAllowed BikesAllowed { get; set; }

        [ForeignKey(nameof(RouteId))]
        public Route Route { get; set; }

        [ForeignKey(nameof(ShapeId))]
        public virtual ShapeBase Shape { get; set; }

        [ForeignKey(nameof(ServiceId))]
        public virtual Calendar Calendar { get; set; }
    }
}