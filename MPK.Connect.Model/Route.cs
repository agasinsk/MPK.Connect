using System;
using System.Collections;
using System.Collections.Generic;

namespace MPK.Connect.Model
{
    public class Route
    {
        public Agency Agency { get; set; }

        public int AgencyId { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        public string LongName { get; set; }

        public RouteType RouteType { get; set; }
        public int RouteTypeId { get; set; }
        public string ShortName { get; set; }
        public int StopTypeId { get; set; }
        public ICollection<Trip> Trips { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public VehicleTypes VehicleType { get; set; }
    }
}