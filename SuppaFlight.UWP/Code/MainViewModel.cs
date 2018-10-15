using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.Storage;
using FlightVisualizer.Core;
using FlightVisualizer.Core.FlightAcademy;
using ReactiveUI;
using SuppaFlight.UWP.Code.Units;

namespace SuppaFlight.UWP.Code
{
    public class MainViewModel : ReactiveObject, IDisposable
    {
        public MainViewModel(FileOpenCommands fileCommands, INavigationService navigationService, IVideoExportService exportService)
        {
            var client = FlightAcademyClient.Create("bla", "ble", new Uri("http://academy.ardrone.com/api3"));

            UnitPack = UnitPacks.First();
            FileCommands = fileCommands;

            var selectedFromFlightAcademy = this.WhenAnyValue(x => x.SelectedFlight)
                .Where(x => x != null)
                .SelectMany(f => client.GetFlight(f.Id))
                .ObserveOnDispatcher()
                .Select(x => x.ToFlightData());

            var datasMerged = fileCommands.OpenDataCommand.Merge(selectedFromFlightAcademy);
            dataOh = datasMerged.ToProperty(this, x => x.Data);
            videoOh = fileCommands.OpenVideoCommand.ToProperty(this, x => x.InputVideo);

            RunCommand = ReactiveCommand.Create(() =>
            {
                navigationService.Navigate(new FlightReplayViewModel(Data.Statuses, InputVideo, unitPack));
                MessageBus.Current.SendMessage(Unit.Default, "Play");
            }, this.WhenAnyValue(x => x.Data, x => x.InputVideo, (data, file) => data != null && file != null));

            hasDataOh = this.WhenAnyValue(x => x.Data).Any(data => data != null).ToProperty(this, x => x.HasData);
            hasVideoOh = this.WhenAnyValue(x => x.InputVideo).Any(data => data != null).ToProperty(this, x => x.HasVideo);

            var selectMany = FileCommands.SaveObs()
                .ObserveOnDispatcher()
                .Select(s => new ExportInput(InputVideo, Data, s));

            ExportVideoCommand = ReactiveCommand.CreateFromObservable(() => selectMany);
            ExportVideoCommand.Subscribe(input =>
            {
                var vm = new ExportViewModel(exportService, input);
                navigationService.Navigate(vm);
            });

            
            LoadFlightsCommand = ReactiveCommand.CreateFromTask(() => client.GetFlights(0, 2000));
            flightsOh = LoadFlightsCommand.ToProperty(this, x => x.Flights);
        }

        public ICollection<Flight> Flights => flightsOh.Value;

        public ReactiveCommand<Unit, ICollection<Flight>> LoadFlightsCommand { get; }

        public StorageFile InputVideo => videoOh.Value;

        public FlightData Data => dataOh.Value;

        public ReactiveCommand<Unit, ExportInput> ExportVideoCommand { get; }

        public bool HasVideo => hasVideoOh.Value;

        public bool HasData => hasDataOh.Value;

        public ReactiveCommand<Unit, Unit> RunCommand { get; }

        public FileOpenCommands FileCommands { get; }

        private readonly ObservableAsPropertyHelper<bool> hasDataOh;
        private readonly ObservableAsPropertyHelper<bool> hasVideoOh;
        private UnitPack unitPack;
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private readonly ObservableAsPropertyHelper<StorageFile> videoOh;
        private readonly ObservableAsPropertyHelper<FlightData> dataOh;
        private readonly ObservableAsPropertyHelper<ICollection<Flight>> flightsOh;
        private Flight selectedFlight;

        public UnitPack UnitPack
        {
            get => unitPack;
            set => this.RaiseAndSetIfChanged(ref unitPack, value);
        }

        public ICollection<UnitPack> UnitPacks => UnitSource.UnitPacks;

        public Flight SelectedFlight
        {
            get => selectedFlight;
            set => this.RaiseAndSetIfChanged(ref selectedFlight, value);
        }

        public void Dispose()
        {
            hasDataOh?.Dispose();
            hasVideoOh?.Dispose();
            disposables?.Dispose();
            RunCommand?.Dispose();
        }
    }
}