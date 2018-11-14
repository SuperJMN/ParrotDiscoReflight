using System;
using System.Linq;
using System.Reactive.Linq;
using ParrotDiscoReflight.Code;
using ParrotDiscoReflight.Controls;
using ReactiveUI;
using Reflight.Core;

namespace ParrotDiscoReflight.ViewModels
{
    public class SimulationViewModel : ReactiveObject, ISimulationViewModel
    {
        private readonly Simulation simulation;
        private readonly ObservableAsPropertyHelper<StatusViewModel> status;

        public SimulationViewModel(Simulation simulation, IObservable<TimeSpan> positionObservable,
            PresentationOptions presentationOptions)
        {
            this.simulation = simulation;
            PresentationOptions = presentationOptions;
            var obs = positionObservable.Select(pos => GetStatus(simulation, pos));

            Speed = new PlottableViewModel(obs.Select(x => new Point((x.TimeElapsed - simulation.Offset).TotalMilliseconds, x.Speed.L2Norm())), simulation.Statuses.Select(s => s.Speed.L2Norm()).ToList());
            Altitude = new PlottableViewModel(obs.Select(x => new Point((x.TimeElapsed - simulation.Offset).TotalMilliseconds, x.DronePosition.Altitude)), simulation.Statuses.Select(s => s.DronePosition.Altitude).ToList());

            status = obs
                .Select(status => new StatusViewModel(status))
                .ToProperty(this, x => x.Status);
        }

        private static Status GetStatus(Simulation simulation, TimeSpan pos)
        {
            return simulation.Statuses
                .SkipWhile(x => x.TimeElapsed < pos)
                .Take(1)
                .DefaultIfEmpty(Reflight.Core.Status.Zero)
                .First();
        }

        public PresentationOptions PresentationOptions { get; }
        public IPlottableViewModel Speed { get; }
        public IPlottableViewModel Altitude { get; }
        public TimeSpan FlightDuration => simulation.Flight.RunTime;
        public TimeSpan CapturedDuration => simulation.Video.RecordedInterval.Value.Duration.ToTimeSpan();
        public StatusViewModel Status => status.Value;
    }
}