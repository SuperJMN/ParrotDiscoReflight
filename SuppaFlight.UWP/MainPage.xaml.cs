using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ReactiveUI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SuppaFlight.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            MessageBus.Current.Listen<StorageFile>().Subscribe(file =>
            {
                var mediaSource = MediaSource.CreateFromStorageFile(file);
                MediaElement.SetPlaybackSource(mediaSource);
            });

           

            MessageBus.Current.Listen<BasicGeoposition>().Subscribe(x =>
            {
                var myLandmarks = new List<MapElement>();

                var snPoint = new Geopoint(x);

                var spaceNeedleIcon = new MapIcon
                {
                    Location = snPoint,
                    NormalizedAnchorPoint = new Point(0.5, 1.0),
                    ZIndex = 0,
                    Title = "Space Needle"
                };

                myLandmarks.Add(spaceNeedleIcon);

                var landmarksLayer = new MapElementsLayer
                {
                    ZIndex = 1,
                    MapElements = myLandmarks
                };

                MapControl.Layers.Add(landmarksLayer);

                MapControl.Center = snPoint;
            });

            MapControl.MapServiceToken = "2ywvE5Sf0Oc0N1S53Jj2~fbOf4w-CQmaDPXG11-Lx-A~Au6RnaxhBKpKQc79NkfwbrdFrrSr1NHNolZKDcpVFKk1eIWqYubIC3eTZsezamPm";
        }
    }
}
