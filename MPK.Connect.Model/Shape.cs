using MPK.Connect.Model.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    [DistinctId]
    public class Shape : IdentifiableEntity<string>
    {
        [Required]
        public override string Id { get; set; }

        public ICollection<ShapePoint> Points { get; set; }
    }
}