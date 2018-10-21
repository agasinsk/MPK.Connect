using MPK.Connect.Service.Helpers;
using Calendar = MPK.Connect.Model.Calendar;

namespace MPK.Connect.Service.Builders
{
    public class CalendarBuilder : BaseEntityBuilder<Calendar>
    {
        public override Calendar Build(string dataString)
        {
            var data = dataString.Replace("\"", "").ToEntityData();

            var serviceId = data[_entityMappings["service_id"]];
            var monday = data[_entityMappings["monday"]] == "1";
            var tuesday = data[_entityMappings["tuesday"]] == "1";
            var wednesday = data[_entityMappings["wednesday"]] == "1";
            var thursday = data[_entityMappings["thursday"]] == "1";
            var friday = data[_entityMappings["friday"]] == "1";
            var saturday = data[_entityMappings["saturday"]] == "1";
            var sunday = data[_entityMappings["sunday"]] == "1";
            var start = GetDate(data[_entityMappings["start_date"]]).GetValueOrDefault();
            var end = GetDate(data[_entityMappings["end_date"]]).GetValueOrDefault();

            var mappedCalendar = new Calendar
            {
                ServiceId = serviceId,
                Monday = monday,
                Tuesday = tuesday,
                Wednesday = wednesday,
                Thursday = thursday,
                Friday = friday,
                Saturday = saturday,
                Sunday = sunday,
                StartDate = start,
                EndDate = end
            };
            return mappedCalendar;
        }
    }
}