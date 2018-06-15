using System;
using System.Collections.Generic;
using System.Text;

namespace MPK.Connect.Model
{
    public class Variant
    {
        public int DisjoinStopId { get; set; }

        public int EquivalentMainVariantId { get; set; }
        public int Id { get; set; }

        public bool IsMain { get; set; }
        public int JoinStopId { get; set; }
    }
}