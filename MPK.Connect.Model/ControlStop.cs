using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    public class ControlStop
    {
        public Stop Stop { get; set; }

        [Key]
        public int StopId { get; set; }

        public Variant Variant { get; set; }
        public int VariantId { get; set; }
    }
}