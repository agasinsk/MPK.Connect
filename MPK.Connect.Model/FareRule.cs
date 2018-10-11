using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    public class FareRule
    {
        [Key]
        public string FareId { get; set; }

        public string RouteId { get; set; }
        public string OriginId { get; set; }
        public string DestinationId { get; set; }
        public string ContainsId { get; set; }
    }
}