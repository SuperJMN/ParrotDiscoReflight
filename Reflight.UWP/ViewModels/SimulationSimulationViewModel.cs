using System;
using ParrotDiscoReflight.Code;
using ParrotDiscoReflight.Code.Units;
using ReactiveUI;

namespace ParrotDiscoReflight.ViewModels
{
    public class SimulationSimulationViewModel : ReactiveObject, ISimulationViewModel
    {
        private StatusViewModel status;
        public UnitPack Units { get; set; }

        public StatusViewModel Status
        {
            get => status;
            set => this.RaiseAndSetIfChanged(ref status, value);
        }

        public PresentationOptions PresentationOptions { get; }
        public PlottableViewModel Speed { get; set; }
        public PlottableViewModel Altitude { get; }
        public TimeSpan FlightDuration { get; }
        public TimeSpan CapturedDuration { get; }
    }
}