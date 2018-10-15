using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using System;
using System.Collections.Generic;

namespace MPK.Connect.Service.Builders
{
    public class CalendarDateBuilder : BaseEntityBuilder<CalendarDate>
    {
        public override CalendarDate Build(string dataString, IDictionary<string, int> mappings)
        {
            var data = dataString.Replace("\"", "").Split(',');

            var serviceId = data[mappings["service_id"]];
            var date = DateTime.Parse(data[mappings["date"]]);
            Enum.TryParse(mappings.ContainsKey("exception_type") ? data[mappings["exception_type"]] : string.Empty, out ExceptionRules exception);

            var calendarDate = new CalendarDate
            {
                ServiceId = serviceId,
                Date = date,
                ExceptionRule = exception
            };
            return calendarDate;
        }
    }
}