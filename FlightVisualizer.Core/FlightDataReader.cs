using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using FlightVisualizer.Core;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using MathNet.Numerics.LinearAlgebra.Storage;
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

            var transformed = Transform(intermediate);

            return transformed;
        }

        private static FlightData Transform(IntermediateData id)
        {
            var transpose = id.details_data.Transpose();
            var headers = id.details_headers;

            var dataProvider = new DataProvider(id.details_data, id.details_headers);
            var times = dataProvider.GetData<object, long>("time", Convert.ToInt64).ToList();
            var altitude = dataProvider.GetData<object, long>("altitude", Convert.ToInt64).ToList();
            var battLevel = dataProvider.GetData<object, double>("battery_level", o => Convert.ToInt64(o) / (double)100).ToList();
            var lat = dataProvider.GetData<object, double>("product_gps_latitude", Convert.ToDouble).ToList();
            var lng = dataProvider.GetData<object, double>("product_gps_longitude", Convert.ToDouble).ToList();
            var spdX = dataProvider.GetData<object, double>("speed_vx", Convert.ToDouble).ToList();
            var spdY = dataProvider.GetData<object, double>("speed_vy", Convert.ToDouble).ToList();
            var spdZ = dataProvider.GetData<object, double>("speed_vz", Convert.ToDouble).ToList();
            var pitotSpeed = dataProvider.GetData<object, double>("pitot_speed", Convert.ToDouble).ToList();

            var statuses = Enumerable.Range(0, times.Count())
                .Select(i => new Status
                {
                    TimeElapsed = TimeSpan.FromMilliseconds(times[i]),
                    Speed = Vector<double>.Build.Dense(new[] { spdX[i], spdY[i], spdZ[i] }),
                    Altitude = altitude[i] >= 0 ? altitude[i] / (double)1000 : 0,
                    PitotSpeed = pitotSpeed[i],
                    Longitude = lng[i],
                    Latitude = lat[i],
                });

            return new FlightData{ Statuses = statuses.ToList(), };
        }
    }


    public class Status
    {
        public TimeSpan TimeElapsed { get; set; }
        public Vector<double> Speed { get; set; }
        public double Altitude { get; set; }
        public double PitotSpeed { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    internal class DataProvider
    {
        private readonly List<string> headers;
        private readonly IList<IEnumerable<object>> source;

        public DataProvider(IEnumerable<List<object>> idDetailsData, List<string> idDetailsHeaders)
        {
            this.source = idDetailsData.Transpose().ToList();
            this.headers = idDetailsHeaders;
        }

        public IEnumerable<TOut> GetData<TIn, TOut>(string header, Func<TIn, TOut> selector)
        {
            var row = source[headers.IndexOf(header)];
            return row.Select(o => selector((TIn)o));
        }
    }

}

