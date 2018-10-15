using MPK.Connect.Model.Helpers;
using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    public class Shape : IdentifiableEntity<string>
    {
        [Required]
        public override string Id { get; set; }

        [Required]
        public double PointLatitude { get; set; }

        [Required]
        public double PointLongitude { get; set; }

        [Required]
        public int PointSequence { get; set; }

        public double? DistTraveled { get; set; }
    }
}