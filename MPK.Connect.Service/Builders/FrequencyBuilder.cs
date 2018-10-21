using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using MPK.Connect.Service.Helpers;
using System;

namespace MPK.Connect.Service.Builders
{
    public class FrequencyBuilder : BaseEntityBuilder<Frequency>
    {
        public override Frequency Build(string dataString)
        {
            var data = dataString.Replace("\"", "").ToEntityData();

            var tripId = data[_entityMappings["trip_id"]];
            var start = GetTime(data[_entityMappings["start_time"]]).GetValueOrDefault();
            var end = GetTime(data[_entityMappings["end_time"]]).GetValueOrDefault();
            var headwaySecs = int.Parse(data[_entityMappings["headway_secs"]]);
            Enum.TryParse(data[_entityMappings["exact_times"]], out ExactTimes exactTimes);

            var frequency = new Frequency
            {
                TripId = tripId,
                StartTime = start,
                EndTime = end,
                HeadwaySecs = headwaySecs,
                ExactTimes = exactTimes
            };
            return frequency;
        }
    }
}