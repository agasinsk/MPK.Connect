using System;
using Newtonsoft.Json;

namespace MPK.Connect.Model.Business.TravelPlan
{
    internal class DateConverter : JsonConverter<DateTime>
    {
        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
        }

        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return (DateTime)reader.Value;
        }
    }
}