using Calendar = MPK.Connect.Model.Calendar;

namespace MPK.Connect.Service.Builders
{
    public class CalendarBuilder : BaseEntityBuilder<Calendar>
    {
        public override Calendar Build(string dataString)
        {
            var calendarData = dataString.Replace("\"", "").Split(',');
            var serviceId = calendarData[_entityMappings["service_id"]];
            var monday = calendarData[_entityMappings["monday"]] == "1";
            var tuesday = calendarData[_entityMappings["tuesday"]] == "1";
            var wednesday = calendarData[_entityMappings["wednesday"]] == "1";
            var thursday = calendarData[_entityMappings["thursday"]] == "1";
            var friday = calendarData[_entityMappings["friday"]] == "1";
            var saturday = calendarData[_entityMappings["saturday"]] == "1";
            var sunday = calendarData[_entityMappings["sunday"]] == "1";
            var start = GetDateTime(calendarData[_entityMappings["start_date"]]).GetValueOrDefault();
            var end = GetDateTime(calendarData[_entityMappings["end_date"]]).GetValueOrDefault();

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