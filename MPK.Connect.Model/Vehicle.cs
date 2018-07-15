using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    public class Vehicle
    {
        public int Description { get; set; }

        [Key]
        public int Id { get; set; }

        public int Name { get; set; }
        public int Symbol { get; set; }
    }
}