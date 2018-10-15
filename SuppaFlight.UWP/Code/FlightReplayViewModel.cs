using System;
using System.Collections.Generic;
using Windows.Storage;
using FlightVisualizer.Core;
using ReactiveUI;
using SuppaFlight.UWP.Code.Units;

namespace SuppaFlight.UWP.Code
{
    public class FlightReplayViewModel : ReactiveObject
    {
        private TimeSpan position;

        public FlightReplayViewModel(IEnumerable<Status> statusObservable, StorageFile video, UnitPack unitPack)
        {
            Video = video;
            DataViewModel = new DataViewModel(statusObservable, this.WhenAnyValue(model => model.Position), unitPack);
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