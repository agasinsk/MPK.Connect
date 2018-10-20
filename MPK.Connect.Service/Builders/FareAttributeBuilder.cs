using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using System;

namespace MPK.Connect.Service.Builders
{
    public class FareAttributeBuilder : BaseEntityBuilder<FareAttribute>
    {
        public override FareAttribute Build(string dataString)
        {
            var data = dataString.Replace("\"", "").Split(',');

            var fareId = data[_entityMappings["fare_id"]];
            var price = double.Parse(data[_entityMappings["price"]]);
            var currency = data[_entityMappings["currency_type"]];
            Enum.TryParse(_entityMappings.ContainsKey("payment_method") ? data[_entityMappings["payment_method"]] : string.Empty, out PaymentMethods payment);
            var transfers = data[_entityMappings["transfers"]] == string.Empty ? (int?)null : int.Parse(data[_entityMappings["transfers"]]);
            var agencyId = _entityMappings.ContainsKey("agency_id") ? data[_entityMappings["agency_id"]] : null;
            var transferDuration = _entityMappings.ContainsKey("transfer_duration") ? long.Parse(data[_entityMappings["transfer_duration"]]) : (long?)null;

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