using MPK.Connect.Model;
using System;
using System.Collections.Generic;

namespace MPK.Connect.Service.Builders
{
    public class CalendarBuilder : BaseEntityBuilder<Calendar>
    {
        public override Calendar Build(string dataString, IDictionary<string, int> mappings)
        {
            var calendarData = dataString.Replace("\"", "").Split(',');
            var serviceId = calendarData[mappings["service_id"]];
            var monday = calendarData[mappings["monday"]] == "1";
            var tuesday = calendarData[mappings["tuesday"]] == "1";
            var wednesday = calendarData[mappings["wednesday"]] == "1";
            var thursday = calendarData[mappings["thursday"]] == "1";
            var friday = calendarData[mappings["friday"]] == "1";
            var saturday = calendarData[mappings["saturday"]] == "1";
            var sunday = calendarData[mappings["sunday"]] == "1";
            var start = DateTime.Parse(calendarData[mappings["start_date"]]);
            var end = DateTime.Parse(calendarData[mappings["end_date"]]);

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