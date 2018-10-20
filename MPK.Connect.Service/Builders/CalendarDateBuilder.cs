using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using System;

namespace MPK.Connect.Service.Builders
{
    public class CalendarDateBuilder : BaseEntityBuilder<CalendarDate>
    {
        public override CalendarDate Build(string dataString)
        {
            var data = dataString.Replace("\"", "").Split(',');

            var serviceId = data[_entityMappings["service_id"]];
            var date = GetDateTime(data[_entityMappings["date"]]).GetValueOrDefault();
            Enum.TryParse(_entityMappings.ContainsKey("exception_type") ? data[_entityMappings["exception_type"]] : string.Empty, out ExceptionRules exception);

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