using System;
using MathNet.Numerics.LinearAlgebra;

namespace FlightVisualizer.Core
{
    public class Status
    {
        public TimeSpan TimeElapsed { get; set; }
        public Vector<double> Speed { get; set; }
        public double Altitude { get; set; }
        public double PitotSpeed { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double AnglePhi { get; set; }
        public double AngleTheta { get; set; }
        public double AnglePsi { get; set; }
        public double BatteryLevel { get; set; }
    }
}