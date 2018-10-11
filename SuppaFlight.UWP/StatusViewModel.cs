using System;
using FlightVisualizer.Core;

namespace SuppaFlight.UWP
{
    public class StatusViewModel
    {
        private readonly Status status;

        public StatusViewModel(Status status)
        {
            this.status = status;
        }

        public double Speed => status.Speed.L2Norm();
        public TimeSpan TimeElapsed => status.TimeElapsed;

        public double Altitude => status.Altitude;

        public double PitotSpeed => status.PitotSpeed;
        public double Longitude => status.Longitude;
        public double Latitude => status.Latitude;
        public double AnglePhi => status.AnglePhi;
        public double AngleTheta => status.AngleTheta;
        public double AnglePsi => status.AnglePsi;
        public double BatteryLevel => status.BatteryLevel;

        public double WifiStrength => status.WifiStregth;
    }
}