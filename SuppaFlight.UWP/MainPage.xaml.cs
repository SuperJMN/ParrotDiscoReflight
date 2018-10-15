using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SuppaFlight.UWP.Code;
using SuppaFlight.UWP.Code.Units;

namespace SuppaFlight.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            var frame = (Frame)Window.Current.Content;

            var navigationService = new NavigationService(frame);
            navigationService.Register<FlightReplayViewModel, FlightReplayPage>();
            navigationService.Register<ExportViewModel, ExportPage>();

            DataContext = new MainViewModel(new FileOpenCommands(), navigationService, new ExportService(status => new SimulationDataViewModel
            {                
                UnitPack = UnitSource.UnitPacks.First(),
                Status = new StatusViewModel(status.ConvertTo(UnitSource.UnitPacks.First()))
            }));
        }
    }
}
