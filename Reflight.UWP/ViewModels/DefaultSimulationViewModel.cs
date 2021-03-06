﻿using System;
using ParrotDiscoReflight.Code;
using ParrotDiscoReflight.Code.Units;
using ReactiveUI;

namespace ParrotDiscoReflight.ViewModels
{
    public class DefaultSimulationViewModel : ReactiveObject, ISimulationViewModel
    {
        public DefaultSimulationViewModel(PresentationOptions presentationOptions)
        {
            PresentationOptions = presentationOptions;
        }

        private StatusViewModel status;
        public UnitPack Units { get; set; }

        public StatusViewModel Status
        {
            get => status;
            set => this.RaiseAndSetIfChanged(ref status, value);
        }

        public PresentationOptions PresentationOptions { get; }
        public IPlottableViewModel Speed { get; set; }
        public IPlottableViewModel Altitude { get; set; }
        public TimeSpan FlightDuration { get; }
        public TimeSpan CapturedDuration { get; }
    }
}