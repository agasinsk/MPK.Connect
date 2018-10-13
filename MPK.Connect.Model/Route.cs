using MPK.Connect.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    public class Route
    {
        [Required]
        public string Id { get; set; }

        public string AgencyId { get; set; }

        [Required]
        public string ShortName { get; set; }

        [Required]
        public string LongName { get; set; }

        public string Description { get; set; }

        [Required]
        public RouteTypes Type { get; set; }

        public string Url { get; set; }
        public string Color { get; set; }
        public string TextColor { get; set; }
        public string SortOrder { get; set; }

        public Agency Agency { get; set; }
    }
}