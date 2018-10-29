using System;
using System.Reactive;
using ReactiveUI;

namespace ParrotDiscoReflight.ViewModels
{
    public class VideoPackItemViewModel : ReactiveObject
    {
        private ObservableAsPropertyHelper<bool> isBusy;

        public VideoPackItemViewModel(SimulationSeed simulationSeed, Action<Simulation> onPlay)
        {
            Video = simulationSeed.Video;
            Duration = simulationSeed.Video.RecordedInterval.Value.Duration.ToTimeSpan();
            Date = simulationSeed.Video.RecordedInterval.Value.Start.ToDateTimeOffset();
            PlayCommand = ReactiveCommand.CreateFromTask(async () => new Simulation(simulationSeed.Video,
                await simulationSeed.GetFlight(),
                simulationSeed.GetUnitPack()));

            PlayCommand.Subscribe(onPlay);
            isBusy = PlayCommand.IsExecuting.ToProperty(this, x => x.IsBusy);
        }

        public bool IsBusy => isBusy.Value;

        public ReactiveCommand<Unit, Simulation> PlayCommand { get; }
        public TimeSpan Duration { get; }
        public DateTimeOffset Date { get; }
        public Video Video { get; }
    }
}