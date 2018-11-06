using MPK.Connect.Model.Helpers;
using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    public class Agency : IdentifiableEntity<string>
    {
        [Key]
        [Required]
        public override string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public string Timezone { get; set; }

        public string Language { get; set; }

        public string Phone { get; set; }
        public string FareUrl { get; set; }
        public string Email { get; set; }
    }
}