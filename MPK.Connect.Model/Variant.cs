using System.Collections.Generic;

namespace MPK.Connect.Model
{
    public class Variant
    {
        public ICollection<ControlStop> ControlStops { get; set; }
        public virtual Stop DisjoinStop { get; set; }
        public int DisjoinStopId { get; set; }
        public int EquivalentMainVariantId { get; set; }
        public int Id { get; set; }

        public bool IsMain { get; set; }

        public virtual Stop JoinStop { get; set; }

        public int JoinStopId { get; set; }

        public ICollection<Trip> Trips { get; set; }
    }
}