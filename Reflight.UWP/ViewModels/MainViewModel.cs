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
        private readonly ObservableAsPropertyHelper<SimulationSeed> simulationUnit;

        private IFlightSimulationPicker selectedPicker;

        public MainViewModel(INavigationService navigationService,
            GalleryPickViewModel galleryPickViewModel, ManualPickViewModel manualPickViewModel,
            SettingsViewModel settingsViewModel, IEnumerable<IFlightSimulationPicker> pickers)
        {
            GalleryPickViewModel = galleryPickViewModel;
            ManualPickViewModel = manualPickViewModel;
            Pickers = pickers;

            GoToSettingsCommand = ReactiveCommand.Create(() => navigationService.Navigate(settingsViewModel));
        }

        public IFlightSimulationPicker SelectedPicker
        {
            get => selectedPicker;
            set => this.RaiseAndSetIfChanged(ref selectedPicker, value);
        }

        public ReactiveCommand<Unit, Unit> GoToSettingsCommand { get; }

        public GalleryPickViewModel GalleryPickViewModel { get; }
        public ManualPickViewModel ManualPickViewModel { get; }
        public IEnumerable<IFlightSimulationPicker> Pickers { get; }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}