using MPK.Connect.Model.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPK.Connect.Model
{
    public class Agency : Identifiable<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [Required]
        public override int Id { get; set; }

        public string Language { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Timezone { get; set; }

        [Required]
        public string Url { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }

        public string FareUrl { get; set; }
    }
}