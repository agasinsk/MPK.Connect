using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    public class Stop
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Latitude { get; set; }

        [Required]
        public string Longitude { get; set; }

        public string Name { get; set; }
        public int StopTypeId { get; set; }

        public override string ToString()
        {
            return $"Stop {Id}:{Name} ";
        }
    }
}