using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using System;

namespace MPK.Connect.Service.Builders
{
    public class FrequencyBuilder : BaseEntityBuilder<Frequency>
    {
        public override Frequency Build(string dataString)
        {
            var data = dataString.Replace("\"", "").Split(',');

            var tripId = data[_entityMappings["trip_id"]];
            var start = GetDate(data[_entityMappings["start_time"]]).GetValueOrDefault();
            var end = GetDate(data[_entityMappings["end_time"]]).GetValueOrDefault();
            var headwaySecs = int.Parse(data[_entityMappings["headway_secs"]]);
            Enum.TryParse(_entityMappings.ContainsKey("exact_times") ? data[_entityMappings["exact_times"]] : string.Empty, out ExactTimes exactTimes);

            var frequency = new Frequency
            {
                Id = tripId,
                StartTime = start,
                EndTime = end,
                HeadwaySecs = headwaySecs,
                ExactTimes = exactTimes
            };
            return frequency;
        }
    }
}