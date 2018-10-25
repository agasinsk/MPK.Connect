using MPK.Connect.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MPK.Connect.Service.Builders
{
    public abstract class BaseEntityBuilder<T> : IEntityBuilder<T> where T : class
    {
        protected const string _dateFormatString = "yyyyMMdd";
        protected const string _timeFormatString = "hh:mm:ss";
        protected MappingsDictionary _entityMappings;

        public abstract T Build(string dataString);

        public virtual IDictionary<string, int> ReadEntityMappings(string headerString)
        {
            var mappings = new MappingsDictionary();

            var headers = headerString.Split(',');
            var headerIndex = 0;
            foreach (var header in headers)
            {
                mappings.Add(header, headerIndex);
                headerIndex++;
            }

            _entityMappings = mappings;

            return mappings;
        }

        protected DateTime? GetDate(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
            {
                return null;
            }
            var dateParsingSuccessful = DateTime.TryParseExact(dateString, _dateFormatString, CultureInfo.CurrentCulture, DateTimeStyles.None, out var parsedDateTime);
            return dateParsingSuccessful ? parsedDateTime : (DateTime?)null;
        }

        protected TimeSpan? GetTime(string timeString)
        {
            if (string.IsNullOrEmpty(timeString))
            {
                return null;
            }
            return TimeSpan.Parse(timeString);
        }


        protected double? GetDouble(string doubleString)
        {
            if (string.IsNullOrEmpty(doubleString))
            {
                return null;
            }

            return double.Parse(doubleString, CultureInfo.InvariantCulture);
        }

        protected int? GetInt(string intString)
        {
            if (string.IsNullOrEmpty(intString))
            {
                return null;
            }

            return int.Parse(intString, CultureInfo.InvariantCulture);
        }
    }
}