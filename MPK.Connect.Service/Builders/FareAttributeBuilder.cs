using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using MPK.Connect.Service.Helpers;
using System;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Builders
{
    public class FareAttributeBuilder : BaseEntityBuilder<FareAttribute>
    {
        public override FareAttribute Build(string dataString)
        {
            var data = dataString.Replace("\"", "").ToEntityData();

            var fareId = data[_entityMappings["fare_id"]];
            var price = GetDouble(data[_entityMappings["price"]]).GetValueOrDefault();
            var currency = data[_entityMappings["currency_type"]];
            Enum.TryParse(data[_entityMappings["payment_method"]], out PaymentMethods payment);
            var transfers = GetNullableInt(data[_entityMappings["transfers"]]);
            var agencyId = data[_entityMappings["agency_id"]];
            var transferDuration = GetNullableInt(data[_entityMappings["transfer_duration"]]);

            var fareAttribute = new FareAttribute
            {
                FareId = fareId,
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