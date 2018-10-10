using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using ReactiveUI;

namespace SuppaFlight.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        private MapIcon spaceNeedleIcon;

        public MainPage()
        {
            InitializeComponent();
            DataContext = new MainViewModel(new FileOpenCommands());            
        }

       
    }
}
