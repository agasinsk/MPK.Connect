using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using MPK.Connect.Service.Helpers;
using System;

namespace MPK.Connect.Service.Builders
{
    public class TransferBuilder : BaseEntityBuilder<Transfer>
    {
        public override Transfer Build(string dataString)
        {
            var data = dataString.Replace("\"", "").ToEntityData();

            var fromStopId = data[_entityMappings["from_stop_id"]];
            var toStopId = data[_entityMappings["to_stop_id"]];
            Enum.TryParse(data[_entityMappings["transfer_type"]], out TransferTypes transferType);
            var minTransferTime = GetInt(data[_entityMappings["min_transfer_time"]]);

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