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

            var settingsService = new SettingsViewModel(fileOpenCommands);

            var videoExportService = new ExportService(status =>
                new SimulationDataViewModel
                {
                    UnitPack = UnitSource.UnitPacks.First(),
                    Status = new StatusViewModel(status.ConvertTo(UnitSource.UnitPacks.First()))
                });

            IFlightAcademyClient FactoryClient() => FlightAcademyClient.Create(settingsService.Username,
                settingsService.Password, new Uri("http://academy.ardrone.com/api3"));

            var dialogService = new DialogService();

            var flightAcademyPickViewModel = new GalleryPickViewModel(FactoryClient, settingsService, dialogService, navigationService);

            var manualPickViewModel = new ManualPickViewModel(fileOpenCommands, FactoryClient, dialogService, settingsService, navigationService);
            var settingsViewModel = new SettingsViewModel(fileOpenCommands);

            return new MainViewModel(navigationService, flightAcademyPickViewModel, manualPickViewModel, settingsViewModel, new IFlightSimulationPicker[]{ flightAcademyPickViewModel, manualPickViewModel });
        }
    }
}