using MPK.Connect.Model.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    [DistinctId]
    public class ShapeBase : IdentifiableEntity<string>
    {
        [Required]
        public override string Id { get; set; }

        public ICollection<Shape> Shapes { get; set; }
    }
}