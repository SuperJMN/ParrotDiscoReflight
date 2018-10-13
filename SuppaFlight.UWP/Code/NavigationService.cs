using Windows.UI.Xaml.Controls;

namespace SuppaFlight.UWP.Code
{
    partial class NavigationService : INavigationService
    {
        private readonly Frame frame;

        public NavigationService(Frame frame)
        {
            this.frame = frame;
        }

        public void Navigate<T>(T viewModel)
        {
            frame.Navigate(typeof(FlightReplayPage), viewModel);
        }
    }
}