using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ParrotDiscoReflight.ViewModels;
using ReactiveUI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ParrotDiscoReflight.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FlightReplayPage : Page
    {
        public FlightReplayPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var vm = (FlightReplayViewModel)e.Parameter;
            DataContext = vm;
            FlightViewer.Video = vm.Video.Source;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
