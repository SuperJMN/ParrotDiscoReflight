using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Reflight.Core;

namespace ParrotDiscoReflight.ViewModels
{
    public class FlightReplayViewModel : ReactiveObject
    {
        private TimeSpan position;
        private DataViewModel dataViewModel;

        public FlightReplayViewModel(Simulation simulation)
        {
            Video = simulation.Video;
            var positions = this.WhenAnyValue(model => model.Position).Select(x => x.Add(simulation.Offset));
            DataViewModel = new DataViewModel(simulation.Flight.Statuses, positions, simulation.UnitPack);
        }

        public ReactiveCommand<Unit, Flight> LoadFlightCommand { get; set; }

        public TimeSpan Position
        {
            get => position;
            set => this.RaiseAndSetIfChanged(ref position, value);
        }

        public DataViewModel DataViewModel
        {
            get => dataViewModel;
            set => this.RaiseAndSetIfChanged(ref dataViewModel, value);
        }

        public Video Video { get; }
    }
}