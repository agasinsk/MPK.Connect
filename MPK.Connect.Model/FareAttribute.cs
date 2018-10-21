using MPK.Connect.Model.Enums;
using MPK.Connect.Model.Helpers;
using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    public class FareAttribute : IdentifiableEntity<string>
    {
        public override string Id => FareId;

        [Required]
        public string FareId { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string CurrencyType { get; set; }

        [Required]
        public PaymentMethods PaymentMethod { get; set; }

        [Required]
        public int? Transfers { get; set; }

        public string AgencyId { get; set; }
        public int? TransferDuration { get; set; }

        public Agency Agency { get; set; }
    }
}