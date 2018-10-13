using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using SuppaFlight.UWP.Code;

namespace SuppaFlight.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = new MainViewModel(new FileOpenCommands(), new Code.NavigationService(this.FindParent<Frame>()));            

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var frame = (Frame) Window.Current.Content;
            DataContext = new MainViewModel(new FileOpenCommands(), new NavigationService(frame));
        }
    }
}
