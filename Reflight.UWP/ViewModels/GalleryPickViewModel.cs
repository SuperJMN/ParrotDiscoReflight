using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.Storage;
using NodaTime;
using NodaTime.Extensions;
using ParrotDiscoReflight.Code;
using ParrotDiscoReflight.Code.Settings;
using ReactiveUI;
using Reflight.Core.FlightAcademy;

namespace ParrotDiscoReflight.ViewModels
{
    public class GalleryPickViewModel : ReactiveObject, IFlightSimulationPicker
    {
        private readonly Func<IFlightAcademyClient> clientFactory;
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private readonly ObservableAsPropertyHelper<bool> isBusy;
        private readonly INavigationService navigationService;
        private readonly SettingsViewModel settingsViewModel;
        private readonly ObservableAsPropertyHelper<IEnumerable<VideoPackViewModel>> videopacks;
        private VideoPackViewModel selectedFlight;

        public GalleryPickViewModel(Func<IFlightAcademyClient> clientFactory, SettingsViewModel settingsViewModel,
            IDialogService dialogService, INavigationService navigationService)
        {
            this.clientFactory = clientFactory;
            this.settingsViewModel = settingsViewModel;
            this.navigationService = navigationService;

            var videoObs = Observable.FromAsync(async () =>
                {
                    var folder = await StorageFolder.GetFolderFromPathAsync(settingsViewModel.VideoFolder);
                    return await folder.GetFilesAsync();
                }).Select(x => x.ToObservable())
                .SelectMany(x => x)
                .SelectMany(Video.Load)
                .Where(x => x.RecordedInterval.HasValue)
                .ToList();

            var summariesObs = Observable.FromAsync(() => clientFactory().GetFlights(0, 2000));

            var zipped = videoObs.Zip(summariesObs,
                (videos, flight) =>
                {
                    return flight.Select((x, i) => CreateSimulationUnitViewModel(x, videos)).Where(x => x.Items.Any());
                });

            var canLoadFlights = settingsViewModel
                .WhenAnyValue(x => x.VideoFolder, x => x.Username, x => x.Password, (videoFolder, user, pass) =>
                    new[] {videoFolder, user, pass}
                        .All(s => !string.IsNullOrEmpty(s)));

            LoadFlightsCommand = ReactiveCommand.CreateFromObservable(() => zipped
                .ObserveOnDispatcher(), canLoadFlights);

            LoadFlightsCommand.ThrownExceptions.MessageOnException(dialogService)
                .DisposeWith(disposables);

            videopacks = LoadFlightsCommand.ToProperty(this, model => model.VideoPacks);
            isBusy = LoadFlightsCommand.IsExecuting.ToProperty(this, x => x.IsBusy);

            MessageBus.Current
                .Listen<Unit>("LoadData")
                .InvokeCommand(LoadFlightsCommand);
        }

        public bool IsBusy => isBusy.Value;

        public ReactiveCommand<Unit, IEnumerable<VideoPackViewModel>> LoadFlightsCommand { get; }

        public IEnumerable<VideoPackViewModel> VideoPacks => videopacks.Value;

        public VideoPackViewModel SelectedPack
        {
            get => selectedFlight;
            set => this.RaiseAndSetIfChanged(ref selectedFlight, value);
        }

        public bool IsAccountConfigured => settingsViewModel.IsAccountConfigured;

        public bool IsVideoFolderConfigured => settingsViewModel.IsVideoFolderConfigured;

        public IObservable<SimulationSeed> NewSimulations { get; }

        public string Name => "Gallery";

        private VideoPackViewModel CreateSimulationUnitViewModel(FlightSummary flightSummary, IList<Video> videos)
        {
            var interval = new Interval(flightSummary.Date.ToInstant(),
                flightSummary.Date.Add(flightSummary.TotalRunTime).ToInstant());
            var matchingVideos = GetVideosInInterval(videos, interval);
            return new VideoPackViewModel
            {
                FlightSummary = flightSummary,
                Items = matchingVideos.Select(video =>
                {
                    var newSimulationUnit = new SummaryBasedSimulationSeed(
                        () => clientFactory().GetFlight(flightSummary.Id), video, () => settingsViewModel.UnitPack);
                    return new VideoPackItemViewModel(newSimulationUnit, OnPlay);
                }).OrderBy(x => x.Date)
            };
        }

        private void OnPlay(Simulation simulation)
        {
            navigationService.Navigate(new FlightReplayViewModel(simulation));
            MessageBus.Current.SendMessage(Unit.Default, "LoadFlightAndPlay");
        }

        private IEnumerable<Video> GetVideosInInterval(IEnumerable<Video> videos, Interval interval)
        {
            return videos.Where(x =>
                interval.Contains(x.RecordedInterval.Value.Start) &&
                interval.Contains(x.RecordedInterval.Value.End.Minus(Duration.FromSeconds(4))));
        }
    }
}