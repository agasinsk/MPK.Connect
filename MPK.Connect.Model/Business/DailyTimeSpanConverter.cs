using System;
using Newtonsoft.Json;

namespace MPK.Connect.Model.Business
{
    internal class DailyTimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString("hh\\:mm"));
        }

        public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return (TimeSpan)reader.Value;
        }
    }
}