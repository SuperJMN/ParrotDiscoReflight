using System;
using System.Linq;
using Windows.UI.Xaml.Controls;
using ParrotDiscoReflight.Code;
using ParrotDiscoReflight.Code.Settings;
using ParrotDiscoReflight.Code.Units;
using ParrotDiscoReflight.ViewModels;
using ParrotDiscoReflight.Views.Pages;
using Reflight.Core.FlightAcademy;

namespace ParrotDiscoReflight
{
    sealed class CompositionRoot
    {
        public static object GetMainContext(Frame rootFrame)
        {
            var navigationService = new NavigationService(rootFrame);
            navigationService.Register<FlightReplayViewModel, FlightReplayPage>();
            navigationService.Register<ExportViewModel, ExportPage>();
            navigationService.Register<SettingsViewModel, SettingsPage>();

            var fileOpenCommands = new FileOpenCommands();

            var settingsViewModel = new SettingsViewModel(fileOpenCommands);

            var videoExportService = new ExportService(status =>
                new SimulationDataViewModel
                {
                    UnitPack = UnitSource.UnitPacks.First(),
                    Status = new StatusViewModel(status.ConvertTo(UnitSource.UnitPacks.First()))
                });

            IFlightAcademyClient FactoryClient() => FlightAcademyClient.Create(settingsViewModel.Username,
                settingsViewModel.Password, new Uri("http://academy.ardrone.com/api3"));

            var dialogService = new DialogService();

            var flightAcademyPickViewModel = new GalleryPickViewModel(FactoryClient, settingsViewModel, dialogService, navigationService);

            var manualPickViewModel = new ManualPickViewModel(fileOpenCommands, FactoryClient, dialogService, settingsViewModel, navigationService);

            return new MainViewModel(navigationService, flightAcademyPickViewModel, manualPickViewModel, settingsViewModel, new IFlightSimulationPicker[]{ flightAcademyPickViewModel, manualPickViewModel });
        }
    }
}