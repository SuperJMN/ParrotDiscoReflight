using System.Linq;
using Windows.Devices.Geolocation;
using FlightVisualizer.Core;

namespace SuppaFlight.UWP
{
    public static class StatusMixin
    {
        public static BasicGeoposition Geoposition(this Status x)
        {
            return new BasicGeoposition
            {
                Altitude = x.Altitude,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
            };
        }

        public static Status ConvertTo(this Status x, IMeasurementUnit unit)
        {
            return new Status(x)
            {
                Speed = new Vector(x.Speed.Coordinates.Select(s => s.ConvertTo(unit)).ToArray()),
                PitotSpeed = x.PitotSpeed.ConvertTo(unit),
            };
        }

        public static double ConvertTo(this double x, IMeasurementUnit unit)
        {
            return unit.Convert(x);
        }
    }
}