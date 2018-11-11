using System;
using System.Reactive.Linq;
using ReactiveUI;

namespace ParrotDiscoReflight.ViewModels
{
    public class FlightReplayViewModel : ReactiveObject
    {
        private TimeSpan position;
        private SimulationViewModel simulationViewModel;

        public FlightReplayViewModel(Simulation simulation)
        {
            Video = simulation.Video;
            var positions = this.WhenAnyValue(model => model.Position).Select(x => x.Add(simulation.Offset));
            SimulationViewModel = new SimulationViewModel(simulation, positions, simulation.PresentationOptions);
        }
        
        public TimeSpan Position
        {
            get => position;
            set => this.RaiseAndSetIfChanged(ref position, value);
        }

        public SimulationViewModel SimulationViewModel
        {
            get => simulationViewModel;
            set => this.RaiseAndSetIfChanged(ref simulationViewModel, value);
        }

        public Video Video { get; }
    }    
}