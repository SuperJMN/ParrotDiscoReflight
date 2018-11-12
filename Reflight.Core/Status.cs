using System;
using GeoCoordinatePortable;

namespace Reflight.Core
{
    public class Status
    {
        public Status()
        {
        }

        public TimeSpan TimeElapsed { get; set; }
        public Vector Speed { get; set; }
        public double PitotSpeed { get; set; }
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
            ControllerPosition = new GeoCoordinate(0,0,123),
            TimeElapsed = TimeSpan.Zero,            
            Speed = Vector.Zero,
        };

        public double TotalDistance { get; set; }
    }
}