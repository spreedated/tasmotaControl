using Newtonsoft.Json;
using System;
using System.Globalization;

namespace TasmotaQuery.Json
{
    public class TimespanConverter : JsonConverter<TimeSpan>
    {
        public const string TimeSpanFormatString = @"dThh\:mm\:ss";

        public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
        {
            var timespanFormatted = $"{value.ToString(TimeSpanFormatString)}";
            writer.WriteValue(timespanFormatted);
        }

        public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            TimeSpan.TryParseExact((string)reader.Value, TimeSpanFormatString, CultureInfo.InvariantCulture, out TimeSpan parsedTimeSpan);
            return parsedTimeSpan;
        }
    }
}
