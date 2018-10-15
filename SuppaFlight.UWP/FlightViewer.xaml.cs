using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.Storage;
using Windows.UI.Xaml;
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


        public static readonly DependencyProperty VideoProperty = DependencyProperty.Register(
            "Video", typeof(StorageFile), typeof(FlightViewer), new PropertyMetadata(default(StorageFile), VideoChanged));

        private static void VideoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var flightViewer = (FlightViewer)d;
            var file = (IStorageFile)e.NewValue;
            var mediaPlaybackSource = MediaSource.CreateFromStorageFile(file);
            
            flightViewer.MediaElement.SetPlaybackSource(mediaPlaybackSource);
        }

        public StorageFile Video
        {
            get { return (StorageFile)GetValue(VideoProperty); }
            set { SetValue(VideoProperty, value); }
        }
    }
}
