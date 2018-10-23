using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using ParrotDiscoReflight.Code;
using ParrotDiscoReflight.Code.Settings;
using ReactiveUI;
using Reflight.Core;
using Reflight.Core.FlightAcademy;

namespace ParrotDiscoReflight.ViewModels
{
    public class GalleryPickViewModel : ReactiveObject, IFlightSimulationPicker
    {
        private readonly ObservableAsPropertyHelper<List<SimulationUnitViewModel>> flights;
        private readonly ObservableAsPropertyHelper<bool> isBusy;
        private readonly SettingsViewModel settingsViewModel;
        private SimulationUnitViewModel selectedFlight;
        private readonly CompositeDisposable disposables = new CompositeDisposable();

        public GalleryPickViewModel(Func<IFlightAcademyClient> clientFactory, SettingsViewModel settingsViewModel, IDialogService dialogService)
        {
            this.settingsViewModel = settingsViewModel;

            var simulationUnitObs = Observable.FromAsync(async () =>
                {
                    var folder = await StorageFolder.GetFolderFromPathAsync(settingsViewModel.VideoFolder);
                    var files = await folder.GetFilesAsync();
                    var flightSummaries = await clientFactory().GetFlights(0, 2000);
                    return new { Flights = flightSummaries, Files = files };
                })
                .Select(flights =>
                {
                    return flights.Flights
                        .ToObservable(RxApp.TaskpoolScheduler)
                        .SelectMany(async x =>
                        {
                            var f = await GetFlight(clientFactory(), x);
                            var v = await FindVideo(x, flights.Files);
                            return new { f, v };
                        }).Where(x => x.v != null)
                        .SelectMany(async x =>
                        {
                            var t = await GetThumbnail(x.v);
                            return new SimulationUnitViewModel(new SimulationUnit(x.f.ToFlight(), x.v), t);
                        })
                        .ToEnumerable();
                });

            Simulations = this.WhenAnyValue(x => x.SelectedViewModel)
                .Where(x => x != null)
                .Select(x => x.SimulationUnit);

            var unitViewModels =
                simulationUnitObs.Select(x => x.OrderByDescending(y => y.SimulationUnit.Flight.Date).ToList());

            var canLoadFlights = settingsViewModel
                .WhenAnyValue(x => x.VideoFolder, x => x.Username, x => x.Password, (videoFolder, user, pass) =>
                    new[] { videoFolder, user, pass }
                        .All(s => !string.IsNullOrEmpty(s)));

            LoadFlightsCommand = ReactiveCommand.CreateFromObservable(() => unitViewModels
                .ObserveOnDispatcher(), canLoadFlights);

            LoadFlightsCommand.ThrownExceptions.MessageOnException(dialogService)
                .DisposeWith(disposables);

            flights = LoadFlightsCommand.ToProperty(this, model => model.SimulationUnitsViewModels);
            isBusy = LoadFlightsCommand.IsExecuting.ToProperty(this, x => x.IsBusy);

            MessageBus.Current
                .Listen<Unit>("LoadData")
                .InvokeCommand(LoadFlightsCommand);
        }

        public bool IsBusy => isBusy.Value;

        public ReactiveCommand<Unit, List<SimulationUnitViewModel>> LoadFlightsCommand { get; }

        public List<SimulationUnitViewModel> SimulationUnitsViewModels => flights.Value;

        public SimulationUnitViewModel SelectedViewModel
        {
            get => selectedFlight;
            set => this.RaiseAndSetIfChanged(ref selectedFlight, value);
        }

        public bool IsAccountConfigured => settingsViewModel.IsAccountConfigured;

        public bool IsVideoFolderConfigured => settingsViewModel.IsVideoFolderConfigured;

        public IObservable<SimulationUnit> Simulations { get; }
        public string Name => "Gallery";

        private async Task<byte[]> GetThumbnail(StorageFile storageFile)
        {
            if (storageFile == null) throw new ArgumentNullException(nameof(storageFile));

            var storageItemThumbnail = await storageFile.GetThumbnailAsync(ThumbnailMode.VideosView);
            if (storageItemThumbnail == null) return null;

            return await storageItemThumbnail.AsStream().ToByteArray();
        }

        private Task<FlightDetails> GetFlight(IFlightAcademyClient client, FlightSummary summary)
        {
            return client.GetFlight(summary.Id);
        }

        private static async Task<StorageFile> FindVideo(FlightSummary flightSummary,
            IEnumerable<StorageFile> filesToMatch)
        {
            var matchingVideo = await filesToMatch.ToObservable()
                .SelectMany(async x =>
                {
                    var durationIn100Ns = await x.GetProperty<ulong?>(StorageFileProperty.Duration);
                    var duration = durationIn100Ns != null ? TimeSpan.FromMilliseconds(0.0001 * (double)durationIn100Ns) : (TimeSpan?)null;

                    return new
                    {
                        File = x,
                        EncodedDate = await x.GetProperty<DateTimeOffset?>(StorageFileProperty.DateEncoded),
                        Duration = duration,
                    };
                })
                .Where(x =>
                {
                    if (x.Duration.HasValue && x.EncodedDate.HasValue)
                    {
                        return MatchesDate(flightSummary.Date, x.EncodedDate.Value) && 
                               MatchesDuration(flightSummary.TotalRunTime, x.Duration.Value);
                    }

                    return false;
                })
                .Select(x => x.File)
                .FirstOrDefaultAsync();

            return matchingVideo;
        }

        private static bool MatchesDuration(TimeSpan one, TimeSpan two)
        {
            return one.Subtract(two).Duration() < TimeSpan.FromMinutes(1);
        }

        private static bool MatchesDate(DateTimeOffset dateTwo, DateTimeOffset dateOne)
        {
            return dateTwo.Subtract(dateOne).Duration() < TimeSpan.FromDays(1);
        }
    }
}