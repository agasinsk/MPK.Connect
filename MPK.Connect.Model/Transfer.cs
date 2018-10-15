using MPK.Connect.Model.Enums;
using MPK.Connect.Model.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPK.Connect.Model
{
    public class Transfer : IdentifiableEntity<string>
    {
        [NotMapped]
        public override string Id => $"{FromStopId}>{ToStopId}";

        [Required]
        public string FromStopId { get; set; }

        [Required]
        public string ToStopId { get; set; }

        public TransferTypes TransferType { get; set; }
        public long? MinTransferTime { get; set; }

        public virtual Stop FromStop { get; set; }

        public virtual Stop ToStop { get; set; }
    }
}