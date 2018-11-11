using System.IO;
using Newtonsoft.Json;
using Reflight.Core.FlightAcademy;

namespace Reflight.Core.Reader
{
    public static class FlightDataReader
    {
        public static Flight Read(Stream stream)
        {
            FlightDetails intermediate;
            using (var reader = new StreamReader(stream))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var ser = new JsonSerializer()
                    {
                        Converters = { new TimeSpanJsonConverter(), new JsonDateTimeOffsetConverter(), }
                    };

                    intermediate = ser.Deserialize<FlightDetails>(jsonReader);
                }
            }

            var transformed = intermediate.ToFlight();

            return transformed;
        }
    }
}

