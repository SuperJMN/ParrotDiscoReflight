using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Reflight.Core
{
    public class JsonDateTimeOffsetConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return DateTimeOffset.ParseExact(reader.Value.ToString(), "yyyy-MM-dd'T'HHmmsszzzz",
                DateTimeFormatInfo.InvariantInfo,
                DateTimeStyles.None);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTimeOffset);
        }
    }
}