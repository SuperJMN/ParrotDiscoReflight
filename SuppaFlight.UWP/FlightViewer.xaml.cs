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
    public sealed partial class FlightViewer
    {
        public FlightViewer()
        {
            InitializeComponent();

            MessageBus.Current.Listen<Unit>("Play").Subscribe(x =>
            {
                MediaElement.Play();
            });

            MessageBus.Current.Listen<StorageFile>().Subscribe(file =>
            {
                var mediaSource = MediaSource.CreateFromStorageFile(file);
                MediaElement.SetPlaybackSource(mediaSource);
            });          
        }
    }
}
