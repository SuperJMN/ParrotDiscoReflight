using System;
using Windows.Storage;
using ParrotDiscoReflight.Code;
using ParrotDiscoReflight.Code.Units;
using ReactiveUI;

namespace ParrotDiscoReflight.ViewModels
{
    public class FlightReplayViewModel : ReactiveObject
    {
        private TimeSpan position;

        public FlightReplayViewModel(SimulationUnit unit, UnitPack unitPack)
        {
            Video = unit.Video;
            DataViewModel = new DataViewModel(unit.Flight.Statuses, this.WhenAnyValue(model => model.Position), unitPack);
        }

        public TimeSpan Position
        {
            get => position;
            set => this.RaiseAndSetIfChanged(ref position, value);
        }

        public DataViewModel DataViewModel { get; }
        public StorageFile Video { get; }
    }
}