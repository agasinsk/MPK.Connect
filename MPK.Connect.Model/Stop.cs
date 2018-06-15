using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    public class Stop
    {
        public int Id { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string Name { get; set; }

        public StopType StopType { get; set; }
        public int StopTypeId { get; set; }

        public override string ToString()
        {
            return $"Stop {Id}:{Name} ";
        }
    }
}