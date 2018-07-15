using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    public class Route
    {
        public Agency Agency { get; set; }

        public int AgencyId { get; set; }
        public string Description { get; set; }

        [Key]
        public int Id { get; set; }

        public string LongName { get; set; }
        public string RouteId { get; set; }
        public RouteType RouteType { get; set; }
        public int RouteTypeId { get; set; }
        public string ShortName { get; set; }
        public int StopTypeId { get; set; }
        public virtual ICollection<Trip> Trips { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public VehicleTypes VehicleType { get; set; }
    }
}