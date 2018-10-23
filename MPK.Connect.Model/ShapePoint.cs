﻿using MPK.Connect.Model.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPK.Connect.Model
{
    public class ShapePoint : IdentifiableEntity<string>
    {
        [NotMapped]
        public override string Id => $"{ShapeId}:{PointSequence}";

        [Required]
        public string ShapeId { get; set; }

        [Required]
        public double PointLatitude { get; set; }

        [Required]
        public double PointLongitude { get; set; }

        [Required]
        public int PointSequence { get; set; }

        public double? DistTraveled { get; set; }

        public Shape Shape { get; set; }
    }
}