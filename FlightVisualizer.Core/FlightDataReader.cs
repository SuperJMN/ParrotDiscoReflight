using System.IO;
using Newtonsoft.Json;

namespace FlightVisualizer.Core
{
    public static class FlightDataReader
    {
        public static FlightData Read(Stream stream)
        {
            IntermediateData intermediate;
            using (var reader = new StreamReader(stream))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var ser = new JsonSerializer();
                    intermediate = ser.Deserialize<IntermediateData>(jsonReader);
                }
            }

            var transformed = intermediate.ToFlightData();

            return transformed;
        }        
    }
}

