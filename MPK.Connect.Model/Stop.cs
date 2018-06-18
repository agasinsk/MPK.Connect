using System.Collections.Generic;

namespace MPK.Connect.Model
{
    public class Stop
    {
        public string Code { get; set; }
        public ICollection<Variant> DisjoinVariants { get; set; }
        public int Id { get; set; }
        public ICollection<Variant> JoinVariants { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; }
        public ICollection<Route> Routes { get; set; }
        public int StopId { get; set; }
        public ICollection<StopTime> StopTimes { get; set; }

        public override string ToString()
        {
            return $"Stop {Id}:{Name}";
        }
    }
}