using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using System;

namespace MPK.Connect.Service.Builders
{
    public class TransferBuilder : BaseEntityBuilder<Transfer>
    {
        public override Transfer Build(string dataString)
        {
            var data = dataString.Replace("\"", "").Split(',');

            var fromStopId = data[_entityMappings["from_stop_id"]];
            var toStopId = data[_entityMappings["to_stop_id"]];
            Enum.TryParse(data[_entityMappings["transfer_type"]], out TransferTypes transferType);
            var minTransferTime = _entityMappings.ContainsKey("min_transfer_time") ? long.Parse(data[_entityMappings["min_transfer_time"]]) : (long?)null;

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