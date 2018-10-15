using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using System;
using System.Collections.Generic;

namespace MPK.Connect.Service.Builders
{
    public class FareAttributeBuilder : BaseEntityBuilder<FareAttribute>
    {
        public override FareAttribute Build(string dataString, IDictionary<string, int> mappings)
        {
            var data = dataString.Replace("\"", "").Split(',');

            var fareId = data[mappings["fare_id"]];
            var price = double.Parse(data[mappings["price"]]);
            var currency = data[mappings["currency_type"]];
            Enum.TryParse(mappings.ContainsKey("payment_method") ? data[mappings["payment_method"]] : string.Empty, out PaymentMethods payment);
            var transfers = data[mappings["transfers"]] == string.Empty ? (int?)null : int.Parse(data[mappings["transfers"]]);
            var agencyId = mappings.ContainsKey("agency_id") ? data[mappings["agency_id"]] : null;
            var transferDuration = mappings.ContainsKey("transfer_duration") ? long.Parse(data[mappings["transfer_duration"]]) : (long?)null;

            var fareAttribute = new FareAttribute
            {
                Id = fareId,
                Price = price,
                CurrencyType = currency,
                PaymentMethod = payment,
                Transfers = transfers,
                AgencyId = agencyId,
                TransferDuration = transferDuration
            };
            return fareAttribute;
        }
    }
}