using System.ComponentModel.DataAnnotations;
using MPK.Connect.Model.Enums;
using MPK.Connect.Model.Helpers;

namespace MPK.Connect.Model
{
    public class Stop : IdentifiableEntity<string>
    {
        [Required]
        public override string Id { get; set; }

        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public string ZoneId { get; set; }
        public string Url { get; set; }
        public LocationTypes LocationType { get; set; }
        public string ParentStation { get; set; }
        public string Timezone { get; set; }
        public WheelchairBoardings WheelchairBoarding { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}