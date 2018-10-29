using System;
using ParrotDiscoReflight.Code.Units;
using Reflight.Core;

namespace ParrotDiscoReflight.ViewModels
{
    public class Simulation
    {
        public Simulation(Video video, Flight flight, UnitPack unitPack)
        {
            Video = video;
            Flight = flight;
            UnitPack = unitPack;
        }

        public Video Video { get; }
        public Flight Flight { get; }
        public UnitPack UnitPack { get; }
        public TimeSpan Offset => Video.RecordedInterval.Value.Start.ToDateTimeOffset().Subtract(Flight.Date);
    }
}