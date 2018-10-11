using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace FlightVisualizer.Core
{
    public static class FlightDataReader
    {
        private static readonly TimeSpan TimeCorrection = TimeSpan.FromSeconds(-1);

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

            var transformed = Transform(intermediate);

            return transformed;
        }

        private static FlightData Transform(IntermediateData id)
        {
            var dataProvider = new DataProvider(id.details_data, id.details_headers);
            var times = dataProvider.GetData<object, long>("time", Convert.ToInt64).ToList();
            var altitude = dataProvider.GetData<object, long>("altitude", Convert.ToInt64).ToList();
            var battLevel = dataProvider.GetData<object, int>("battery_level", Convert.ToInt32).ToList();
            var lat = dataProvider.GetData<object, double>("product_gps_latitude", Convert.ToDouble).ToList();
            var lng = dataProvider.GetData<object, double>("product_gps_longitude", Convert.ToDouble).ToList();
            var spdX = dataProvider.GetData<object, double>("speed_vx", Convert.ToDouble).ToList();
            var spdY = dataProvider.GetData<object, double>("speed_vy", Convert.ToDouble).ToList();
            var spdZ = dataProvider.GetData<object, double>("speed_vz", Convert.ToDouble).ToList();
            var pitotSpeed = dataProvider.GetData<object, double>("pitot_speed", Convert.ToDouble).ToList();
            var anglePhi = dataProvider.GetData<object, double>("angle_phi", Convert.ToDouble).ToList();
            var angleTheta = dataProvider.GetData<object, double>("angle_theta", Convert.ToDouble).ToList();
            var anglePsi = dataProvider.GetData<object, double>("angle_psi", Convert.ToDouble).ToList();
            var wifiStrength = dataProvider.GetData<object, double>("wifi_signal", Convert.ToDouble).ToList();
            
            var statuses = Enumerable.Range(0, times.Count)
                .Select(i => new Status
                {
                    TimeElapsed = TimeSpan.FromMilliseconds(times[i]).Add(TimeCorrection),
                    Speed = new Vector(spdX[i], spdY[i], spdZ[i]),
                    Altitude = altitude[i] >= 0 ? altitude[i] / (double)1000 : 0,
                    PitotSpeed = pitotSpeed[i],
                    Longitude = lng[i],
                    Latitude = lat[i],
                    AnglePhi = anglePhi[i],
                    AngleTheta = angleTheta[i],
                    AnglePsi = anglePsi[i],
                    BatteryLevel = battLevel[i] / 100D,
                    WifiStregth = wifiStrength[i],
                });

            return new FlightData { Statuses = statuses.ToList(), };
        }
    }
}

