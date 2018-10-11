using MPK.Connect.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    public class Transfer
    {
        [Required]
        public string FromStopId { get; set; }

        [Required]
        public string ToStopId { get; set; }

        public TransferTypes TransferType { get; set; }
        public long MinTransferTime { get; set; }
    }
}