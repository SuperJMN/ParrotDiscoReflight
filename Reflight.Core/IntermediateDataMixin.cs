using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using GeoCoordinatePortable;
using Reflight.Core.Reader;

namespace Reflight.Core
{
    public static class IntermediateDataMixin
    {
        private static readonly TimeSpan TimeCorrection = TimeSpan.FromSeconds(-0.7);

        public static Flight ToFlight(this FlightDetails data)
        {
            var dataProvider = new DataProvider(data.DetailsData, data.DetailsHeaders);
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
            var ctrlLat = dataProvider.GetData<object, double>("controller_gps_latitude", Convert.ToDouble).ToList();
            var ctrlLng = dataProvider.GetData<object, double>("controller_gps_longitude", Convert.ToDouble).ToList();

            var statuses = Observable.Range(0, times.Count)
                .Where(i => IsValidGpsCoord(lat[i]) && IsValidGpsCoord(lng[i]) && IsValidGpsCoord(ctrlLng[i]) &&
                            IsValidGpsCoord(ctrlLat[i]))
                .Select(i => new RawData
                {
                    TimeElapsed = TimeSpan.FromMilliseconds(times[i]).Add(TimeCorrection),
                    Speed = new Vector(spdX[i], spdY[i], spdZ[i]),
                    Altitude = altitude[i] >= 0 ? altitude[i] / (double) 1000 : 0,
                    PitotSpeed = pitotSpeed.Any() ? pitotSpeed[i] : double.NaN, 
                    Longitude = lng[i],
                    Latitude = lat[i],
                    AnglePhi = anglePhi[i],
                    AngleTheta = angleTheta[i],
                    AnglePsi = anglePsi[i],
                    BatteryLevel = battLevel[i] / 100D,
                    WifiStregth = wifiStrength[i],
                    DronePosition = new GeoCoordinate(lat[i], lng[i], altitude[i] / 1000D),
                    ControllerPosition = new GeoCoordinate(ctrlLat[i], ctrlLng[i], 0D),
                })
                .Buffer(2, 1)
                .SkipLast(1)
                .Scan(new DifferentialData {Distance = 0D, Element = new List<RawData>()}, (actual, states) => new DifferentialData
                {
                    Distance = actual.Distance + states[0].DronePosition.GetDistanceTo(states[1].DronePosition),
                    Element = states,
                })
                .Select(x =>
                {
                    var current = x.Element[1];

                    return new Status
                    {
                        Longitude = current.Longitude,
                        Altitude = current.Altitude,
                        DronePosition = current.DronePosition,
                        Latitude = current.Altitude,
                        ControllerPosition = current.ControllerPosition,
                        TimeElapsed = current.TimeElapsed,
                        AnglePsi = current.AnglePsi,
                        Speed = current.Speed,
                        AngleTheta = current.AngleTheta,
                        WifiStregth = current.WifiStregth,
                        PitotSpeed = current.PitotSpeed,
                        AnglePhi = current.AnglePhi,
                        BatteryLevel = current.BatteryLevel,
                        TotalDistance = x.Distance,
                    };
                });

            var collection = statuses.ToEnumerable().ToList();
            return new Flight
            {
                Date = data.Date,
                RunTime = data.RunTime,
                TotalRunTime = data.TotalRunTime,
                Statuses = collection,
            };
        }

        private static bool IsValidGpsCoord(double d)
        {
            return d >= -90 && d <= 90;
        }

        private class RawData
        {
            public TimeSpan TimeElapsed { get; set; }
            public Vector Speed { get; set; }
            public double Altitude { get; set; }
            public double PitotSpeed { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public double AnglePhi { get; set; }
            public double AngleTheta { get; set; }
            public double AnglePsi { get; set; }
            public double BatteryLevel { get; set; }
            public double WifiStregth { get; set; }
            public GeoCoordinate DronePosition { get; set; }
            public GeoCoordinate ControllerPosition { get; set; }    }

        private class DifferentialData
        {
            public double Distance { get; set; }
            public IList<RawData> Element { get; set; }
        }
    }    
}