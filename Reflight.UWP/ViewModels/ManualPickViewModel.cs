using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.Storage;
using ParrotDiscoReflight.Code;
using ParrotDiscoReflight.Code.Settings;
using ReactiveUI;
using Reflight.Core;
using Reflight.Core.FlightAcademy;

namespace ParrotDiscoReflight.ViewModels
{
    public class ManualPickViewModel : ReactiveObject, IFlightSimulationPicker
    {
        private readonly ObservableAsPropertyHelper<Flight> flightOh;

        private readonly ObservableAsPropertyHelper<ICollection<FlightSummary>> flightsOh;
        private readonly ObservableAsPropertyHelper<bool> hasDataOh;
        private readonly ObservableAsPropertyHelper<bool> hasVideoOh;

        private readonly ObservableAsPropertyHelper<StorageFile> videoOh;
        private FlightSummary selectedFlightSummary;
        private readonly ObservableAsPropertyHelper<SimulationUnit> selectedFlightOh;
        private readonly CompositeDisposable disposables = new CompositeDisposable();

        public ManualPickViewModel(FileOpenCommands fileCommands, Func<IFlightAcademyClient> clientFactory,
            IDialogService dialogService)
        {
            FileCommands = fileCommands;
            var selectedFromFlightAcademy = this.WhenAnyValue(x => x.SelectedFlightSummary)
                .Where(x => x != null)
                .SelectMany(f => clientFactory().GetFlight(f.Id))
                .Select(x => x.ToFlight())
                .ObserveOnDispatcher()
                .Select(x => x);

            var datasMerged = fileCommands.OpenDataCommand.Merge(selectedFromFlightAcademy);
            flightOh = datasMerged.ToProperty(this, x => x.Flight);
            videoOh = fileCommands.OpenVideoCommand.ToProperty(this, x => x.InputVideo);
            
            hasDataOh = this.WhenAnyValue(x => x.Flight, selector: x => x!=null).ToProperty(this, x => x.HasData);
            hasVideoOh = this.WhenAnyValue(x => x.InputVideo).Any(data => data != null)
                .ToProperty(this, x => x.HasVideo);

            LoadFlightsCommand = ReactiveCommand.CreateFromTask(() => clientFactory().GetFlights(0, 2000));
            LoadFlightsCommand.ThrownExceptions.MessageOnException(dialogService)
                .DisposeWith(disposables);
            flightsOh = LoadFlightsCommand.ToProperty(this, x => x.Flights);

            var flights = this.WhenAnyValue(x => x.Flight).Where(x => x != null);
            var videos = this.WhenAnyValue(x => x.InputVideo).Where(s => s != null);
            SimulationUnits = flights.CombineLatest(videos, (d, v) => new SimulationUnit(d, v));

            selectedFlightOh = SimulationUnits.ToProperty(this, x => x.SelectedSimulation);

            var canRun = this.WhenAnyValue(x => x.Flight, x => x.InputVideo, (flight, file) => flight != null && file != null);
            RunCommand = ReactiveCommand.Create(() => new SimulationUnit(Flight, InputVideo), canRun);
        }

        public SimulationUnit SelectedSimulation => selectedFlightOh.Value;
        public IObservable<SimulationUnit> Simulations => SimulationUnits;
        public string Name => "Manual pick";

        public ReactiveCommand<Unit, SimulationUnit> RunCommand { get; }

        public IObservable<SimulationUnit> SimulationUnits { get; }

        public ReactiveCommand<Unit, ICollection<FlightSummary>> LoadFlightsCommand { get; }

        public StorageFile InputVideo => videoOh.Value;

        public ICollection<FlightSummary> Flights => flightsOh.Value;

        public Flight Flight => flightOh.Value;

        public FlightSummary SelectedFlightSummary
        {
            get => selectedFlightSummary;
            set => this.RaiseAndSetIfChanged(ref selectedFlightSummary, value);
        }

        public bool HasData => hasDataOh.Value;
        public bool HasVideo => hasVideoOh.Value;
        public FileOpenCommands FileCommands { get; }        
    }
}