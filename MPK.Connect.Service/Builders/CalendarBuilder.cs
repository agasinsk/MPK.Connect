using System;
using System.Collections.Generic;
using System.Globalization;
using Calendar = MPK.Connect.Model.Calendar;

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
            var start = DateTime.ParseExact(calendarData[mappings["start_date"]], _dateFormatString, CultureInfo.CurrentCulture);
            var end = DateTime.ParseExact(calendarData[mappings["start_date"]], _dateFormatString, CultureInfo.CurrentCulture);

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