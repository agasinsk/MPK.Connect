using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    public class Variant
    {
        public virtual Stop DisjoinStop { get; set; }
        public int DisjoinStopId { get; set; }
        public int EquivalentMainVariantId { get; set; }

        [Key]
        public int Id { get; set; }

        public bool IsMain { get; set; }

        public virtual Stop JoinStop { get; set; }

        public int JoinStopId { get; set; }

        public ICollection<Trip> Trips { get; set; }
    }
}