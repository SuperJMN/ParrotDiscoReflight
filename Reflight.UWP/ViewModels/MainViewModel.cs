using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using ParrotDiscoReflight.Code;
using ParrotDiscoReflight.Code.Settings;
using ReactiveUI;

namespace ParrotDiscoReflight.ViewModels
{
    public class MainViewModel : ReactiveObject, IDisposable
    {
        private readonly CompositeDisposable disposables = new CompositeDisposable();

        private IFlightSimulationPicker selectedPicker;
        private readonly ObservableAsPropertyHelper<SimulationUnit> simulationUnit;

        public MainViewModel(INavigationService navigationService,
            GalleryPickViewModel galleryPickViewModel, ManualPickViewModel manualPickViewModel, SettingsViewModel settingsViewModel
            , IEnumerable<IFlightSimulationPicker> pickers)
        {
            GalleryPickViewModel = galleryPickViewModel;
            ManualPickViewModel = manualPickViewModel;
            Pickers = pickers;
           
            GoToSettingsCommand = ReactiveCommand.Create(() => navigationService.Navigate(settingsViewModel));
            var simulationUnits = this.WhenAnyObservable(model => model.SelectedPicker.Simulations);
            simulationUnit = simulationUnits.ToProperty(this, x => x.SimulationUnit);
            
            RunCommand = ReactiveCommand.Create(() => { },
                this.WhenAnyValue(x => x.SimulationUnit, selector: x => x != null));

            RunCommand.Subscribe(x =>
            {
                navigationService.Navigate(new FlightReplayViewModel(SimulationUnit, settingsViewModel.UnitPack));
                MessageBus.Current.SendMessage(Unit.Default, "Play");
            });
        }
        
        public IFlightSimulationPicker SelectedPicker
        {
            get => selectedPicker;
            set => this.RaiseAndSetIfChanged(ref selectedPicker, value);
        }

        public ReactiveCommand<Unit, Unit> GoToSettingsCommand { get; }

        public SimulationUnit SimulationUnit => simulationUnit.Value;

        public ReactiveCommand<Unit, ExportInput> ExportVideoCommand { get; }
        public ReactiveCommand<Unit, Unit> RunCommand { get; }         
        public GalleryPickViewModel GalleryPickViewModel { get;  }
        public ManualPickViewModel ManualPickViewModel { get; }
        public IEnumerable<IFlightSimulationPicker> Pickers { get; }
        
        public void Dispose()
        {
            disposables?.Dispose();
            RunCommand?.Dispose();
        }
    }
}