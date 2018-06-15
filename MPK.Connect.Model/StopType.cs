using System.Collections.Generic;

namespace MPK.Connect.Model
{
    public class StopType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Stop> Stops { get; set; }
    }
}