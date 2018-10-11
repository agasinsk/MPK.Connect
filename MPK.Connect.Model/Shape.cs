using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    public class Shape
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string PointLatitude { get; set; }

        [Required]
        public string PointLongitude { get; set; }

        [Required]
        public int PointSequence { get; set; }

        public double DistTraveled { get; set; }
    }
}