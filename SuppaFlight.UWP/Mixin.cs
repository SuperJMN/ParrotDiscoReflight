using Windows.Devices.Geolocation;
using FlightVisualizer.Core;

namespace SuppaFlight.UWP
{
    public static class Mixin
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
    }
}