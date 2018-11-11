using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ParrotDiscoReflight.ViewModels;
using ParrotDiscoReflight.Views.Pages;
using ParrotDiscoReflight.Views.Settings;
using Reflight.Core.FlightAcademy;

namespace ParrotDiscoReflight.Code
{
    sealed class CompositionRoot
    {
        public static object GetMainContext(Frame rootFrame)
        {
            var navigationService = new NavigationService(rootFrame);
            navigationService.Register<FlightReplayViewModel, FlightReplayPage>();
            navigationService.Register<SettingsViewModel, SettingsPage>();

            var fileOpenCommands = new FileOpenCommands();
            
            var dialogService = new DialogService();
            var settingsViewModel = new SettingsViewModel(fileOpenCommands,dialogService, new VirtualDashboardRepository(new ResourceDictionary() { Source = new Uri("ms-appx:///Views/VirtualDashboards.xaml") }));
           
            Task<IFlightAcademyClient> FactoryClient() => FlightAcademyClient.Create(settingsViewModel.Username,
                settingsViewModel.Password, new Uri("http://academy.ardrone.com/api3"));


            var flightAcademyPickViewModel = new GalleryPickViewModel(FactoryClient, settingsViewModel, dialogService, navigationService);

            var manualPickViewModel = new ManualPickViewModel(fileOpenCommands, FactoryClient, dialogService, settingsViewModel, navigationService);

            return new MainViewModel(navigationService, flightAcademyPickViewModel, manualPickViewModel, settingsViewModel, new IFlightSimulationPicker[]{ flightAcademyPickViewModel, manualPickViewModel });
        }
    }
}