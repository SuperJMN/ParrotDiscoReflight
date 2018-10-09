using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml.Controls.Maps;
using ReactiveUI;

namespace SuppaFlight.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Main
    {
        private MapIcon spaceNeedleIcon;

        public Main()
        {
            InitializeComponent();
            DataContext = new MainViewModel2(new FileOpenCommands());

            MessageBus.Current.Listen<Unit>("Play").Subscribe(x => MediaElement.Play());

            MessageBus.Current.Listen<StorageFile>().Subscribe(file =>
            {
                var mediaSource = MediaSource.CreateFromStorageFile(file);
                MediaElement.SetPlaybackSource(mediaSource);
            });

            AddPin();

            MessageBus.Current.Listen<BasicGeoposition>("First")
                .Do(TrySetLocation)
                .Do(_ => MapControl.ZoomLevel = 14)
                .Subscribe(p => MapControl.Center = new Geopoint(p));

            MessageBus.Current.Listen<BasicGeoposition>().Subscribe(TrySetLocation);

            MapControl.MapServiceToken = "2ywvE5Sf0Oc0N1S53Jj2~fbOf4w-CQmaDPXG11-Lx-A~Au6RnaxhBKpKQc79NkfwbrdFrrSr1NHNolZKDcpVFKk1eIWqYubIC3eTZsezamPm";
        }

        private void TrySetLocation(BasicGeoposition x)
        {
            try
            {
                spaceNeedleIcon.Location = new Geopoint(x);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void AddPin()
        {
            var myLandmarks = new List<MapElement>();

            spaceNeedleIcon = new MapIcon
            {
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                ZIndex = 0,
            };

            myLandmarks.Add(spaceNeedleIcon);

            var landmarksLayer = new MapElementsLayer
            {
                ZIndex = 1,
                MapElements = myLandmarks
            };

            MapControl.Layers.Add(landmarksLayer);
        }
    }
}
