using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using System;
using System.Collections.Generic;

namespace MPK.Connect.Service.Builders
{
    public class FrequencyBuilder : BaseEntityBuilder<Frequency>
    {
        public override Frequency Build(string dataString, IDictionary<string, int> mappings)
        {
            var data = dataString.Replace("\"", "").Split(',');

            var tripId = data[mappings["trip_id"]];
            var start = DateTime.Parse(data[mappings["start_time"]]);
            var end = DateTime.Parse(data[mappings["end_time"]]);
            var headwaySecs = int.Parse(data[mappings["headway_secs"]]);
            Enum.TryParse(mappings.ContainsKey("exact_times") ? data[mappings["exact_times"]] : string.Empty, out ExactTimes exactTimes);

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