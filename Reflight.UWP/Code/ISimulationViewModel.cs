using System;
using ParrotDiscoReflight.ViewModels;

namespace ParrotDiscoReflight.Code
{
    public interface ISimulationViewModel
    {
        StatusViewModel Status { get; }
        PresentationOptions PresentationOptions { get; }
        PlottableViewModel Speed { get; }
        PlottableViewModel Altitude { get; }
        TimeSpan FlightDuration { get; }
        TimeSpan CapturedDuration { get; }
    }
}
