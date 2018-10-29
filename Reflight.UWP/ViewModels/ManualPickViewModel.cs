﻿using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
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
        private readonly ObservableAsPropertyHelper<Video> video;
        private readonly ObservableAsPropertyHelper<StorageFile> flightFile;
        private readonly ObservableAsPropertyHelper<ICollection<FlightSummary>> summaries;
        private FlightSummary selectedFlightSummary;

        public ManualPickViewModel(FileOpenCommands fileCommands, Func<IFlightAcademyClient> clientFactory,
            IDialogService dialogService, SettingsViewModel settingsViewModel, INavigationService navigationService)
        {
            FileCommands = fileCommands;

            video = fileCommands.OpenVideoCommand.ToProperty(this, x => x.Video);
            flightFile = fileCommands.OpenDataCommand.ToProperty(this, x => x.FlightFile);

            LoadFlightsCommand = ReactiveCommand.CreateFromTask(() => GetFlights(clientFactory));
            summaries = GetFlights(clientFactory)
                .ToObservable()
                .Merge(LoadFlightsCommand)
                .ToProperty(this, x => x.FlightSummaries);

            var manualSimulations = this
                .WhenAnyValue(x => x.Video, x => x.FlightFile, (v, f) => new {Video = v, FlightFile = f})
                .SelectMany(async x =>
                {
                    var readFlight = await x.FlightFile.ReadFlight();
                    return new Simulation(x.Video, readFlight, settingsViewModel.UnitPack);
                })
                .FirstOrDefaultAsync();

            PlayFromFileCommand = ReactiveCommand.CreateFromObservable(() => manualSimulations,
                this.WhenAnyValue(x => x.FlightFile, x => x.Video, (f, v) => f != null && v != null));

            var onlineSimulations = this
                .WhenAnyValue(x => x.Video, x => x.SelectedFlightSummary, (v, s) => new {Video = v, FlightSummary = s})
                .SelectMany(async x =>
                {
                    var readFlight = await clientFactory().GetFlight(x.FlightSummary.Id);
                    return new Simulation(x.Video, readFlight.ToFlight(), settingsViewModel.UnitPack);
                })
                .FirstOrDefaultAsync();

            PlayFromOnlineFlightCommand = ReactiveCommand.CreateFromObservable(() => onlineSimulations,
                this.WhenAnyValue(x => x.SelectedFlightSummary, x => x.Video, (f, v) => f != null && v != null));

            PlayFromFileCommand.Subscribe(simulation => { PlaySimulation(navigationService, simulation); });
            PlayFromOnlineFlightCommand.Subscribe(simulation => { PlaySimulation(navigationService, simulation); });
        }

        private static void PlaySimulation(INavigationService navigationService, Simulation simulation)
        {
            navigationService.Navigate(new FlightReplayViewModel(simulation));
            MessageBus.Current.SendMessage(Unit.Default, "LoadFlightAndPlay");
        }

        public ReactiveCommand<Unit, Simulation> PlayFromOnlineFlightCommand { get; set; }

        private static Task<ICollection<FlightSummary>> GetFlights(Func<IFlightAcademyClient> clientFactory)
        {
            return clientFactory().GetFlights(0, 2000);
        }

        public FlightSummary SelectedFlightSummary
        {
            get => selectedFlightSummary;
            set => this.RaiseAndSetIfChanged(ref selectedFlightSummary, value);
        }

        public ICollection<FlightSummary> FlightSummaries => summaries.Value;

        public StorageFile FlightFile => flightFile.Value;

        public Video Video => video.Value;

        public FileOpenCommands FileCommands { get; }

        public ReactiveCommand<Unit, Simulation> PlayFromFileCommand { get; }
        public string Name => "Manual pick";

        public bool IsLoadingFlight { get; set; }

        public ReactiveCommand<Unit, ICollection<FlightSummary>> LoadFlightsCommand { get; }
    }
}