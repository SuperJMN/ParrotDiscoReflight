using System.Linq;
using Windows.Devices.Geolocation;
using ParrotDiscoReflight.Code.Units;
using Reflight.Core;

namespace ParrotDiscoReflight.Code
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

        //public static Status ConvertTo(this Status x, Units unitPack)
        //{
        //    return new Status(x)
        //    {
        //        Speed = new Vector(x.Speed.Coordinates.Select(s => s.ConvertTo(unitPack.Speed)).ToArray()),
        //        PitotSpeed = x.PitotSpeed.ConvertTo(unitPack.Speed),
        //        Altitude = x.Altitude.ConvertTo(unitPack.Longitude),
        //    };
        //}

        private static double ConvertTo(this double x, IMeasurementUnit unit)
        {
            return unit.Convert(x);
        }
    }
}