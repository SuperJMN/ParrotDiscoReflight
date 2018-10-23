using System;
using GeoCoordinatePortable;
using Reflight.Core;

namespace ParrotDiscoReflight.ViewModels
{
    public class StatusViewModel
    {
        private readonly Status status;

        public StatusViewModel(Status status)
        {
            this.status = status;
        }

        public TimeSpan TimeElapsed => status.TimeElapsed;
        public double Speed => status.Speed.L2Norm();
        public double PitotSpeed => status.PitotSpeed;
        public double AnglePhi => status.AnglePhi;
        public double AngleTheta => status.AngleTheta;
        public double AnglePsi => status.AnglePsi;
        public double BatteryLevel => status.BatteryLevel;

        public double WifiStrength => status.WifiStregth;
        public GeoCoordinate DronePosition => status.DronePosition;
        public GeoCoordinate ControllerPosition => status.ControllerPosition;
        public double DistanceToDrone => IsUnknown(ControllerPosition) ? double.NaN : DronePosition.GetDistanceTo(ControllerPosition);

        private bool IsUnknown(GeoCoordinate controllerPosition)
        {
            var tolerance = 0.1;
            return Math.Abs(controllerPosition.Latitude) < tolerance && Math.Abs(controllerPosition.Longitude) < tolerance;
        }

        public double TotalDistance => status.TotalDistance;
    }
}