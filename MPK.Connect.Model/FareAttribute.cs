﻿using MPK.Connect.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace MPK.Connect.Model
{
    public class FareAttribute
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string CurrencyType { get; set; }

        [Required]
        public PaymentMethods PaymentMethod { get; set; }

        [Required]
        public int? Transfers { get; set; }

        public string AgencyId { get; set; }
        public long TransferDuration { get; set; }

        public Agency Agency { get; set; }
    }
}