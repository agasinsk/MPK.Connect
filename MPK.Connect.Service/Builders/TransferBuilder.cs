using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using System;
using System.Collections.Generic;

namespace MPK.Connect.Service.Builders
{
    public class TransferBuilder : BaseEntityBuilder<Transfer>
    {
        public override Transfer Build(string dataString, IDictionary<string, int> mappings)
        {
            var data = dataString.Replace("\"", "").Split(',');

            var fromStopId = data[mappings["from_stop_id"]];
            var toStopId = data[mappings["to_stop_id"]];
            Enum.TryParse(data[mappings["transfer_type"]], out TransferTypes transferType);
            var minTransferTime = mappings.ContainsKey("min_transfer_time") ? long.Parse(data[mappings["min_transfer_time"]]) : (long?)null;

            var transfer = new Transfer
            {
                FromStopId = fromStopId,
                ToStopId = toStopId,
                TransferType = transferType,
                MinTransferTime = minTransferTime
            };
            return transfer;
        }
    }
}