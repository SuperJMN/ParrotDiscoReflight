using System;
using System.Globalization;
using System.Linq;
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
            var formats = new[] { "yyyyMMddHHmmss", "yyyy-MM-dd'T'HHmmsszzzz"};

            var result = DateTimeOffset.TryParseExact(reader.Value.ToString(), formats, DateTimeFormatInfo.InvariantInfo,
                DateTimeStyles.None, out var dateTime);

            if (!result)
            {
                return existingValue;
            }

            return dateTime;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTimeOffset);
        }
    }
}