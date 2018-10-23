using System;
using Newtonsoft.Json;

namespace Reflight.Core.FlightAcademy
{
    public class TimeSpanJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var timeSpan = TimeSpan.FromMilliseconds(Convert.ToDouble(reader.Value));
            return timeSpan;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(TimeSpan))
            {
                return true;
            }

            return false;
        }
    }
}