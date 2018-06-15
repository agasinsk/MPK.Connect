using System;

namespace MPK.Connect.Model
{
    public class Route
    {
        public int AgencyId { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        public string LongName { get; set; }

        public int RouteTypeId { get; set; }

        public string ShortName { get; set; }

        public int StopTypeId { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}