using System;
using GeoCoordinatePortable;

namespace Reflight.Core
{
    public class Status
    {
        public Status()
        {
        }

        public Status(Status other)
        {
            TimeElapsed = other.TimeElapsed;
            Speed = other.Speed;
            Altitude = other.Altitude;
            PitotSpeed = other.PitotSpeed;
            Longitude = other.Longitude;
            Latitude = other.Latitude;
            AnglePhi = other.AnglePhi;
            AngleTheta = other.AngleTheta;
            AnglePsi = other.AnglePsi;
            BatteryLevel = other.BatteryLevel;
            WifiStregth = other.WifiStregth;
            DronePosition = new GeoCoordinate(other.DronePosition.Latitude, other.DronePosition.Longitude, other.Altitude);
            ControllerPosition = new GeoCoordinate(other.ControllerPosition.Latitude, other.ControllerPosition.Longitude, other.ControllerPosition.Altitude);
            TotalDistance = other.TotalDistance;
        }

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
        public GeoCoordinate ControllerPosition { get; set; }

        public static Status Zero => new Status
        {
            DronePosition = new GeoCoordinate(0,0,0),
            ControllerPosition = new GeoCoordinate(0,0,0),
            TimeElapsed = TimeSpan.Zero,            
            Speed = Vector.Zero,
        };

        public double TotalDistance { get; set; }
    }
}