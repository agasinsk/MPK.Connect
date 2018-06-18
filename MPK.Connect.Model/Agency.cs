using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    public class Agency
    {
        public int Id { get; set; }
        public string Language { get; set; }

        [Required]
        public string Name { get; set; }

        public string Phone { get; set; }

        [Required]
        public string Timezone { get; set; }

        [Required]
        public string Url { get; set; }
    }
}