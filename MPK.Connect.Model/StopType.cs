using System.Collections.Generic;

namespace MPK.Connect.Model
{
    public class StopType
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public List<Stop> Stops { get; set; }
    }
}