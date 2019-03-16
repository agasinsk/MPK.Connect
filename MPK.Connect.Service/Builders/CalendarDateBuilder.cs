using MPK.Connect.Model;
using MPK.Connect.Model.Enums;
using MPK.Connect.Service.Helpers;
using System;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Builders
{
    public class CalendarDateBuilder : BaseEntityBuilder<CalendarDate>
    {
        public override CalendarDate Build(string dataString)
        {
            var data = dataString.Replace("\"", "").ToEntityData();

            var serviceId = data[_entityMappings["service_id"]];
            var date = GetDate(data[_entityMappings["date"]]).GetValueOrDefault();
            Enum.TryParse(data[_entityMappings["exception_type"]], out ExceptionRules exception);

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